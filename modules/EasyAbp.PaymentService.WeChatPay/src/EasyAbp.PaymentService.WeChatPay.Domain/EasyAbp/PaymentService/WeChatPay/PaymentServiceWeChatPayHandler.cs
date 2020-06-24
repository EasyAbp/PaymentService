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
        private readonly IClock _clock;
        private readonly IDataFilter _dataFilter;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentServiceWeChatPayHandler(
            IClock clock,
            IDataFilter dataFilter,
            IPaymentRecordRepository paymentRecordRepository,
            IPaymentRepository paymentRepository)
        {
            _clock = clock;
            _dataFilter = dataFilter;
            _paymentRecordRepository = paymentRecordRepository;
            _paymentRepository = paymentRepository;
        }
        
        public virtual async Task HandleAsync(WeChatPayHandlerContext context)
        {
            var dict = context.WeChatRequestXmlData.SelectSingleNode("xml").ToDictionary() ??
                       throw new NullReferenceException();

            if (dict["return_code"] != "SUCCESS" || dict["device_info"] != PaymentServiceWeChatPayConsts.DeviceInfo)
            {
                return;
            }

            using var disabledDataFilter = _dataFilter.Disable<IMultiTenant>();

            var paymentId = Guid.Parse(dict["out_trade_no"] ??
                                       throw new XmlDocumentMissingRequiredElementException("out_trade_no"));
            
            var payment = await _paymentRepository.GetAsync(paymentId);

            payment.SetExternalTradingCode(dict["transaction_id"] ??
                                           throw new XmlDocumentMissingRequiredElementException("transaction_id"));
            
            if (dict["result_code"] == "SUCCESS")
            {
                payment.CompletePayment(_clock.Now);
            }
            else
            {
                payment.CancelPayment(_clock.Now);
            }

            await _paymentRepository.UpdateAsync(payment, true);

            // Todo: Failure also needs to be recorded.
            await RecordPaymentResultAsync(dict, payment.Id);
        }

        protected virtual async Task<PaymentRecord> RecordPaymentResultAsync(Dictionary<string, string> dict, Guid paymentId)
        {
            var couponCount = ConvertToNullableInt32(dict["coupon_count"]);
            
            var record = await _paymentRecordRepository.GetAsync(x => x.PaymentId == paymentId);
            
            record.SetResult(
                returnCode: dict["return_code"],
                returnMsg: dict["return_msg"],
                appId: dict["appid"],
                mchId: dict["mch_id"],
                deviceInfo: dict["device_info"],
                resultCode: dict["result_code"],
                errCode: dict["err_code"],
                errCodeDes: dict["err_code_des"],
                openid: dict["openid"],
                isSubscribe: dict["is_subscribe"],
                tradeType: dict["trade_type"],
                bankType: dict["bank_type"],
                totalFee: ConvertToInt32(dict["total_fee"]),
                settlementTotalFee: ConvertToNullableInt32(dict["total_fee"]),
                feeType: dict["fee_type"],
                cashFee: ConvertToInt32(dict["cash_fee"]),
                cashFeeType: dict["cash_fee_type"],
                couponFee: ConvertToNullableInt32(dict["coupon_fee"]),
                couponCount: couponCount,
                couponTypes: couponCount != null ? dict.JoinNodesInnerTextAsString("coupon_type_", couponCount.Value) : null,
                couponIds: couponCount != null ? dict.JoinNodesInnerTextAsString("coupon_id_", couponCount.Value) : null,
                couponFees: couponCount != null ? dict.JoinNodesInnerTextAsString("coupon_fee_", couponCount.Value) : null,
                transactionId: dict["transaction_id"],
                outTradeNo: dict["out_trade_no"],
                attach: dict["attach"],
                timeEnd: dict["time_end"]
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