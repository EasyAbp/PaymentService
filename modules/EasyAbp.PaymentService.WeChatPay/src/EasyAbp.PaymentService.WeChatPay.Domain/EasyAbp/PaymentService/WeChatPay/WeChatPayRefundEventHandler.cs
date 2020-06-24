using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using EasyAbp.Abp.WeChat.Pay.Services.Pay;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class WeChatPayRefundEventHandler : IWeChatPayRefundEventHandler, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IRefundRecordRepository _refundRecordRepository;
        private readonly IWeChatPayFeeConverter _weChatPayFeeConverter;
        private readonly ServiceProviderPayService _serviceProviderPayService;

        public WeChatPayRefundEventHandler(
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            IRefundRepository refundRepository,
            IPaymentManager paymentManager,
            IPaymentRepository paymentRepository,
            IPaymentRecordRepository paymentRecordRepository,
            IRefundRecordRepository refundRecordRepository,
            IWeChatPayFeeConverter weChatPayFeeConverter,
            ServiceProviderPayService serviceProviderPayService)
        {
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _refundRepository = refundRepository;
            _paymentManager = paymentManager;
            _paymentRepository = paymentRepository;
            _paymentRecordRepository = paymentRecordRepository;
            _refundRecordRepository = refundRecordRepository;
            _weChatPayFeeConverter = weChatPayFeeConverter;
            _serviceProviderPayService = serviceProviderPayService;
        }
        
        [UnitOfWork(true)]
        public virtual async Task HandleEventAsync(WeChatPayRefundEto eventData)
        {
            // Todo: Handle errors and rollback
            using (_currentTenant.Change(eventData.TenantId))
            {
                var payment = await _paymentRepository.GetAsync(eventData.PaymentId);
                var paymentRecord = await _paymentRecordRepository.GetByPaymentId(eventData.PaymentId);
                var refundRecordId = _guidGenerator.Create();

                var dict = await RequestWeChatPayRefundAsync(payment, paymentRecord, eventData, refundRecordId.ToString());

                var externalTradingCode = dict.GetOrDefault("refund_id");
                
                foreach (var refund in eventData.Refunds)
                {
                    refund.SetExternalTradingCode(externalTradingCode);

                    await _refundRepository.UpdateAsync(refund, true);
                }
                
                if (dict.GetOrDefault("result_code") != "SUCCESS")
                {
                    await _paymentManager.RollbackRefundAsync(payment, eventData.Refunds);
                }
            }
        }

        protected virtual async Task CreateWeChatPayRefundRecordEntitiesAsync(Payment payment, Dictionary<string, string> dict)
        {
            var settlementTotalFeeString = dict.GetOrDefault("settlement_total_fee");
            var settlementRefundFeeString = dict.GetOrDefault("settlement_refund_fee");
            var cashRefundFeeString = dict.GetOrDefault("cash_refund_fee");
            var couponRefundFeeString = dict.GetOrDefault("coupon_refund_fee");
            var couponRefundCountString = dict.GetOrDefault("coupon_refund_count");
            var couponRefundCount = couponRefundCountString.IsNullOrEmpty() ? (int?) null : Convert.ToInt32(couponRefundCountString);

            await _refundRecordRepository.InsertAsync(new RefundRecord(
                id: _guidGenerator.Create(),
                tenantId: _currentTenant.Id,
                paymentId: payment.Id,
                returnCode: dict.GetOrDefault("return_code"),
                returnMsg: dict.GetOrDefault("return_msg"),
                appId: dict.GetOrDefault("appid"),
                mchId: dict.GetOrDefault("mch_id"),
                transactionId: dict.GetOrDefault("transaction_id"),
                outTradeNo: dict.GetOrDefault("out_trade_no"),
                refundId: dict.GetOrDefault("refund_id"),
                outRefundNo: dict.GetOrDefault("out_refund_no"),
                totalFee: Convert.ToInt32(dict.GetOrDefault("total_fee")),
                settlementTotalFee: settlementTotalFeeString.IsNullOrEmpty() ? (int?) null : Convert.ToInt32(settlementTotalFeeString),
                refundFee: Convert.ToInt32(dict.GetOrDefault("refund_fee")),
                settlementRefundFee: settlementRefundFeeString.IsNullOrEmpty() ? (int?) null : Convert.ToInt32(settlementRefundFeeString),
                feeType: dict.GetOrDefault("fee_type"),
                cashFee: Convert.ToInt32(dict.GetOrDefault("cash_fee")),
                cashFeeType: dict.GetOrDefault("cash_fee_type"),
                cashRefundFee: cashRefundFeeString.IsNullOrEmpty() ? (int?) null : Convert.ToInt32(cashRefundFeeString),
                couponRefundFee: couponRefundFeeString.IsNullOrEmpty() ? (int?) null : Convert.ToInt32(couponRefundFeeString),
                couponRefundCount: couponRefundCount,
                couponTypes: couponRefundCount != null ? dict.JoinNodesInnerTextAsString("coupon_type_", couponRefundCount.Value) : null,
                couponIds: couponRefundCount != null ? dict.JoinNodesInnerTextAsString("coupon_id_", couponRefundCount.Value) : null,
                couponRefundFees: couponRefundCount != null ? dict.JoinNodesInnerTextAsString("coupon_refund_fee_", couponRefundCount.Value) : null
            ), true);
        }

        private async Task<Dictionary<string, string>> RequestWeChatPayRefundAsync(Payment payment, PaymentRecord paymentRecord, WeChatPayRefundEto eventData, string outRefundNo)
        {
            var refundAmount = eventData.Refunds.Sum(model => model.RefundAmount);

            var result = await _serviceProviderPayService.RefundAsync(
                appId: payment.GetProperty<string>("appid"),
                mchId: payment.PayeeAccount,
                subAppId: null,
                subMchId: null,
                transactionId: paymentRecord.TransactionId,
                outTradeNo: payment.Id.ToString("N"),
                outRefundNo: outRefundNo,
                totalFee: paymentRecord.TotalFee,
                refundFee: _weChatPayFeeConverter.ConvertToWeChatPayFee(refundAmount),
                refundFeeType: null,
                refundDesc: eventData.DisplayReason,
                refundAccount: null,
                notifyUrl: null
            );
            
            var dict = new Dictionary<string, string>(result.SelectSingleNode("xml").ToDictionary() ?? throw new NullReferenceException());

            if (dict.GetOrDefault("return_code") != "SUCCESS")
            {
                throw new RefundFailedException(dict.GetOrDefault("return_code"), dict.GetOrDefault("return_msg"));
            }

            await CreateWeChatPayRefundRecordEntitiesAsync(payment, dict);

            return dict;
        }
    }
}