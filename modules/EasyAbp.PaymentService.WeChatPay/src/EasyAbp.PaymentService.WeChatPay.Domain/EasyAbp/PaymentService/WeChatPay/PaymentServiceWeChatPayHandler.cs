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
            var xmlDocument = context.WeChatRequestXmlData;
            
            var reader = new XmlNodeReader(xmlDocument.SelectSingleNode("xml") ?? throw new NullReferenceException());

            if (reader["device_info"] != PaymentServiceWeChatPayConsts.DeviceInfo)
            {
                return;
            }

            using var disabledDataFilter = _dataFilter.Disable<IMultiTenant>();

            var paymentId = Guid.Parse(reader["out_trade_no"] ??
                                       throw new XmlDocumentMissingRequiredElementException("out_trade_no"));
            
            var payment = await _paymentRepository.GetAsync(paymentId);

            payment.SetExternalTradingCode(reader["transaction_id"] ??
                                           throw new XmlDocumentMissingRequiredElementException("transaction_id"));
            
            if (reader["return_code"] == "SUCCESS")
            {
                payment.CompletePayment(_clock.Now);
            }
            else
            {
                payment.CancelPayment(_clock.Now);
            }

            await _paymentRepository.UpdateAsync(payment, true);

            // Todo: Failure also needs to be recorded.
            await RecordPaymentResultAsync(reader, payment.Id);
        }

        protected virtual async Task<PaymentRecord> RecordPaymentResultAsync(XmlNodeReader reader, Guid paymentId)
        {
            var couponCount = ConvertToNullableInt32(reader["coupon_count"]);
            
            var record = await _paymentRecordRepository.GetAsync(x => x.PaymentId == paymentId);
            
            record.SetResult(
                returnCode: reader["return_code"],
                returnMsg: reader["return_msg"],
                appId: reader["appid"],
                mchId: reader["mch_id"],
                deviceInfo: reader["device_info"],
                resultCode: reader["result_code"],
                errCode: reader["err_code"],
                errCodeDes: reader["err_code_des"],
                openid: reader["openid"],
                isSubscribe: reader["is_subscribe"],
                tradeType: reader["trade_type"],
                bankType: reader["bank_type"],
                totalFee: ConvertToInt32(reader["total_fee"]),
                settlementTotalFee: ConvertToNullableInt32(reader["total_fee"]),
                feeType: reader["fee_type"],
                cashFee: ConvertToInt32(reader["cash_fee"]),
                cashFeeType: reader["cash_fee_type"],
                couponFee: ConvertToNullableInt32(reader["coupon_fee"]),
                couponCount: couponCount,
                couponTypes: couponCount != null ? reader.JoinNodesInnerTextAsString("coupon_type_", couponCount.Value) : null,
                couponIds: couponCount != null ? reader.JoinNodesInnerTextAsString("coupon_id_", couponCount.Value) : null,
                couponFees: couponCount != null ? reader.JoinNodesInnerTextAsString("coupon_fee_", couponCount.Value) : null,
                transactionId: reader["transaction_id"],
                outTradeNo: reader["out_trade_no"],
                attach: reader["attach"],
                timeEnd: reader["time_end"]
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