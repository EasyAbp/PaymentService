using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay.Options;
using EasyAbp.Abp.WeChat.Pay.Services;
using EasyAbp.Abp.WeChat.Pay.Services.Pay;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.WeChatPay.Background
{
    public class WeChatPayRefundJob : IAsyncBackgroundJob<WeChatPayRefundJobArgs>, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly ILogger<WeChatPayRefundJob> _logger;
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IRefundRecordRepository _refundRecordRepository;
        private readonly IWeChatPayFeeConverter _weChatPayFeeConverter;
        private readonly IAbpWeChatPayOptionsProvider _abpWeChatPayOptionsProvider;
        private readonly IAbpWeChatPayServiceFactory _abpWeChatPayServiceFactory;

        public WeChatPayRefundJob(
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            ILogger<WeChatPayRefundJob> logger,
            IRefundRepository refundRepository,
            IPaymentManager paymentManager,
            IPaymentRepository paymentRepository,
            IPaymentRecordRepository paymentRecordRepository,
            IRefundRecordRepository refundRecordRepository,
            IWeChatPayFeeConverter weChatPayFeeConverter,
            IAbpWeChatPayOptionsProvider abpWeChatPayOptionsProvider,
            IAbpWeChatPayServiceFactory abpWeChatPayServiceFactory)
        {
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _logger = logger;
            _refundRepository = refundRepository;
            _paymentManager = paymentManager;
            _paymentRepository = paymentRepository;
            _paymentRecordRepository = paymentRecordRepository;
            _refundRecordRepository = refundRecordRepository;
            _weChatPayFeeConverter = weChatPayFeeConverter;
            _abpWeChatPayOptionsProvider = abpWeChatPayOptionsProvider;
            _abpWeChatPayServiceFactory = abpWeChatPayServiceFactory;
        }

        [UnitOfWork(true)]
        public virtual async Task ExecuteAsync(WeChatPayRefundJobArgs args)
        {
            using var change = _currentTenant.Change(args.TenantId);

            var payment = await _paymentRepository.GetAsync(args.PaymentId);

            // Try to lock the row in DB.
            await _paymentRepository.UpdateAsync(payment, true);

            Refund refund = null;

            if (args.RefundId is not null)
            {
                refund = await _refundRepository.GetAsync(args.RefundId.Value);

                // Try to lock the row in DB.
                await _refundRepository.UpdateAsync(refund, true);

                if (payment.TenantId != refund.TenantId ||
                    payment.Id != refund.PaymentId ||
                    !refund.IsInProgress())
                {
                    throw new RefundIsInUnexpectedStageException(refund.Id);
                }
            }

            var paymentRecord = await _paymentRecordRepository.GetByPaymentId(payment.Id);

            Dictionary<string, string> dict;

            try
            {
                dict = await RequestWeChatPayRefundAsync(payment, paymentRecord, args.RefundAmount, args.OutRefundNo,
                    args.DisplayReason);
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                throw;
            }

            if (refund is not null)
            {
                var externalTradingCode = dict.GetOrDefault("refund_id");

                refund.SetExternalTradingCode(externalTradingCode);

                await _refundRepository.UpdateAsync(refund, true);

                if (dict.GetOrDefault("result_code") != "SUCCESS")
                {
                    await _paymentManager.RollbackRefundAsync(payment, refund);
                }
            }
        }

        protected virtual async Task CreateWeChatPayRefundRecordEntitiesAsync(Payment payment,
            Dictionary<string, string> dict)
        {
            var settlementTotalFeeString = dict.GetOrDefault("settlement_total_fee");
            var settlementRefundFeeString = dict.GetOrDefault("settlement_refund_fee");
            var cashRefundFeeString = dict.GetOrDefault("cash_refund_fee");
            var couponRefundFeeString = dict.GetOrDefault("coupon_refund_fee");
            var couponRefundCountString = dict.GetOrDefault("coupon_refund_count");
            var couponRefundCount = couponRefundCountString.IsNullOrEmpty()
                ? (int?)null
                : Convert.ToInt32(couponRefundCountString);

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
                settlementTotalFee: settlementTotalFeeString.IsNullOrEmpty()
                    ? null
                    : Convert.ToInt32(settlementTotalFeeString),
                refundFee: Convert.ToInt32(dict.GetOrDefault("refund_fee")),
                settlementRefundFee: settlementRefundFeeString.IsNullOrEmpty()
                    ? null
                    : Convert.ToInt32(settlementRefundFeeString),
                feeType: dict.GetOrDefault("fee_type"),
                cashFee: Convert.ToInt32(dict.GetOrDefault("cash_fee")),
                cashFeeType: dict.GetOrDefault("cash_fee_type"),
                cashRefundFee: cashRefundFeeString.IsNullOrEmpty() ? null : Convert.ToInt32(cashRefundFeeString),
                couponRefundFee: couponRefundFeeString.IsNullOrEmpty()
                    ? null
                    : Convert.ToInt32(couponRefundFeeString),
                couponRefundCount: couponRefundCount,
                couponTypes: couponRefundCount != null
                    ? dict.JoinNodesInnerTextAsString("coupon_type_", couponRefundCount.Value)
                    : null,
                couponIds: couponRefundCount != null
                    ? dict.JoinNodesInnerTextAsString("coupon_id_", couponRefundCount.Value)
                    : null,
                couponRefundFees: couponRefundCount != null
                    ? dict.JoinNodesInnerTextAsString("coupon_refund_fee_", couponRefundCount.Value)
                    : null
            ), true);
        }

        private async Task<Dictionary<string, string>> RequestWeChatPayRefundAsync(Payment payment,
            PaymentRecord paymentRecord, decimal refundAmount, [NotNull] string outRefundNo,
            [CanBeNull] string displayReason)
        {
            var mchId = payment.PayeeAccount;
            var options = await _abpWeChatPayOptionsProvider.GetAsync(mchId);

            var serviceProviderPayService =
                await _abpWeChatPayServiceFactory.CreateAsync<ServiceProviderPayWeService>(mchId);
            
            var result = await serviceProviderPayService.RefundAsync(
                appId: payment.GetProperty<string>("appid"),
                mchId: mchId,
                subAppId: null,
                subMchId: null,
                transactionId: paymentRecord.TransactionId,
                outTradeNo: payment.Id.ToString("N"),
                outRefundNo: outRefundNo,
                totalFee: paymentRecord.TotalFee,
                refundFee: _weChatPayFeeConverter.ConvertToWeChatPayFee(refundAmount),
                refundFeeType: null,
                refundDesc: displayReason,
                refundAccount: null,
                notifyUrl: options.RefundNotifyUrl
            );

            var dict = new Dictionary<string, string>(result.SelectSingleNode("xml").ToDictionary() ??
                                                      throw new NullReferenceException());

            if (dict.GetOrDefault("return_code") != "SUCCESS")
            {
                throw new RefundFailedException(dict.GetOrDefault("return_code"), dict.GetOrDefault("return_msg"));
            }

            await CreateWeChatPayRefundRecordEntitiesAsync(payment, dict);

            return dict;
        }
    }
}