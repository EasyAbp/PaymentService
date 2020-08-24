using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using EasyAbp.Abp.WeChat.Pay.Infrastructure;
using EasyAbp.Abp.WeChat.Pay.Infrastructure.OptionResolve;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class PaymentServiceWeChatPayHandler : IWeChatPayHandler, ITransientDependency
    {
        public WeChatHandlerType Type { get; } = WeChatHandlerType.Normal;

        private readonly IDataFilter _dataFilter;
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentServiceWeChatPayHandler(
            IDataFilter dataFilter,
            IPaymentManager paymentManager,
            IPaymentRecordRepository paymentRecordRepository,
            IPaymentRepository paymentRepository)
        {
            _dataFilter = dataFilter;
            _paymentManager = paymentManager;
            _paymentRecordRepository = paymentRecordRepository;
            _paymentRepository = paymentRepository;
        }
        
        public virtual async Task HandleAsync(WeChatPayHandlerContext context)
        {
            var dict = context.WeChatRequestXmlData.SelectSingleNode("xml").ToDictionary() ??
                       throw new NullReferenceException();

            if (dict.GetOrDefault("return_code") != "SUCCESS" || dict.GetOrDefault("device_info") != PaymentServiceWeChatPayConsts.DeviceInfo)
            {
                context.IsSuccess = false;

                return;
            }

            using var disabledDataFilter = _dataFilter.Disable<IMultiTenant>();

            var paymentId = Guid.Parse(dict.GetOrDefault("out_trade_no") ??
                                       throw new XmlDocumentMissingRequiredElementException("out_trade_no"));
            
            var payment = await _paymentRepository.GetAsync(paymentId);

            payment.SetExternalTradingCode(dict.GetOrDefault("transaction_id") ??
                                           throw new XmlDocumentMissingRequiredElementException("transaction_id"));
            
            await _paymentRepository.UpdateAsync(payment, true);

            await RecordPaymentResultAsync(dict, payment.Id);

            if (dict.GetOrDefault("result_code") == "SUCCESS")
            {
                await _paymentManager.CompletePaymentAsync(payment);
            }
            else
            {
                await _paymentManager.StartCancelAsync(payment);
            }
            
            context.IsSuccess = true;
        }
        
        protected virtual async Task<PaymentRecord> RecordPaymentResultAsync(Dictionary<string, string> dict, Guid paymentId)
        {
            var couponCount = ConvertToNullableInt32(dict.GetOrDefault("coupon_count"));
            
            var record = await _paymentRecordRepository.GetAsync(x => x.PaymentId == paymentId);
            
            record.SetResult(
                returnCode: dict.GetOrDefault("return_code"),
                returnMsg: dict.GetOrDefault("return_msg"),
                appId: dict.GetOrDefault("appid"),
                mchId: dict.GetOrDefault("mch_id"),
                deviceInfo: dict.GetOrDefault("device_info"),
                resultCode: dict.GetOrDefault("result_code"),
                errCode: dict.GetOrDefault("err_code"),
                errCodeDes: dict.GetOrDefault("err_code_des"),
                openid: dict.GetOrDefault("openid"),
                isSubscribe: dict.GetOrDefault("is_subscribe"),
                tradeType: dict.GetOrDefault("trade_type"),
                bankType: dict.GetOrDefault("bank_type"),
                totalFee: ConvertToInt32(dict.GetOrDefault("total_fee")),
                settlementTotalFee: ConvertToNullableInt32(dict.GetOrDefault("total_fee")),
                feeType: dict.GetOrDefault("fee_type"),
                cashFee: ConvertToInt32(dict.GetOrDefault("cash_fee")),
                cashFeeType: dict.GetOrDefault("cash_fee_type"),
                couponFee: ConvertToNullableInt32(dict.GetOrDefault("coupon_fee")),
                couponCount: couponCount,
                couponTypes: couponCount != null ? dict.JoinNodesInnerTextAsString("coupon_type_", couponCount.Value) : null,
                couponIds: couponCount != null ? dict.JoinNodesInnerTextAsString("coupon_id_", couponCount.Value) : null,
                couponFees: couponCount != null ? dict.JoinNodesInnerTextAsString("coupon_fee_", couponCount.Value) : null,
                transactionId: dict.GetOrDefault("transaction_id"),
                outTradeNo: dict.GetOrDefault("out_trade_no"),
                attach: dict.GetOrDefault("attach"),
                timeEnd: dict.GetOrDefault("time_end")
            );

            return await _paymentRecordRepository.UpdateAsync(record, true);
        }

        private static int? ConvertToNullableInt32(string text)
        {
            if (text.IsNullOrEmpty())
            {
                return null;
            }

            return Convert.ToInt32(text);
        }

        private static int ConvertToInt32(string text)
        {
            return Convert.ToInt32(text);
        }
    }
}