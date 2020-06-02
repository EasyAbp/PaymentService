using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using EasyAbp.Abp.WeChat.Pay.Infrastructure;
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
        private readonly IGuidGenerator _guidGenerator;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentServiceWeChatPayHandler(
            IClock clock,
            IDataFilter dataFilter,
            IGuidGenerator guidGenerator,
            IPaymentRecordRepository paymentRecordRepository,
            IPaymentRepository paymentRepository)
        {
            _clock = clock;
            _dataFilter = dataFilter;
            _guidGenerator = guidGenerator;
            _paymentRecordRepository = paymentRecordRepository;
            _paymentRepository = paymentRepository;
        }
        
        public virtual async Task HandleAsync(XmlDocument xmlDocument)
        {
            using var disabledDataFilter = _dataFilter.Disable<IMultiTenant>();

            var xml = xmlDocument.SelectSingleNode("xml") ??
                      throw new XmlDocumentMissingRequiredElementException("xml");

            if (xml["return_code"]?.InnerText != "SUCCESS")
            {
                return;
            }

            // Todo: sign check

            var paymentId = Guid.Parse(xml["out_trade_no"]?.InnerText ??
                                     throw new XmlDocumentMissingRequiredElementException("out_trade_no"));
            
            var payment = await _paymentRepository.GetAsync(paymentId);

            payment.SetExternalTradingCode(xml["transaction_id"]?.InnerText ??
                                           throw new XmlDocumentMissingRequiredElementException("transaction_id"));
            
            if (xml["result_code"]?.InnerText == "SUCCESS")
            {
                payment.CompletePayment(_clock.Now);
            }
            else
            {
                payment.CancelPayment(_clock.Now);
            }

            await _paymentRepository.UpdateAsync(payment, true);

            await RecordPaymentResultAsync(xml, payment.Id);
        }

        protected virtual async Task<PaymentRecord> RecordPaymentResultAsync(XmlNode xml, Guid paymentId)
        {
            var couponCount = ConvertToNullableInt32(xml["coupon_count"]?.InnerText);
            
            var record = await _paymentRecordRepository.GetAsync(x => x.PaymentId == paymentId);
            
            record.SetResult(
                returnCode: xml["return_code"]?.InnerText,
                returnMsg: xml["return_msg"]?.InnerText,
                appId: xml["appid"]?.InnerText,
                mchId: xml["mch_id"]?.InnerText,
                deviceInfo: xml["device_info"]?.InnerText,
                nonceStr: xml["nonce_str"]?.InnerText,
                sign: xml["sign"]?.InnerText,
                signType: xml["sign_type"]?.InnerText,
                resultCode: xml["result_code"]?.InnerText,
                errCode: xml["err_code"]?.InnerText,
                errCodeDes: xml["err_code_des"]?.InnerText,
                openid: xml["openid"]?.InnerText,
                isSubscribe: xml["is_subscribe"]?.InnerText,
                tradeType: xml["trade_type"]?.InnerText,
                bankType: xml["bank_type"]?.InnerText,
                totalFee: ConvertToInt32(xml["total_fee"]?.InnerText),
                settlementTotalFee: ConvertToNullableInt32(xml["total_fee"]?.InnerText),
                feeType: xml["fee_type"]?.InnerText,
                cashFee: ConvertToInt32(xml["cash_fee"]?.InnerText),
                cashFeeType: xml["cash_fee_type"]?.InnerText,
                couponFee: ConvertToNullableInt32(xml["coupon_fee"]?.InnerText),
                couponCount: couponCount,
                couponTypes: couponCount != null ? JoinNodesInnerTextAsString(xml, "coupon_type_", couponCount.Value) : null,
                couponIds: couponCount != null ? JoinNodesInnerTextAsString(xml, "coupon_id_", couponCount.Value) : null,
                couponFees: couponCount != null ? JoinNodesInnerTextAsString(xml, "coupon_fee_", couponCount.Value) : null,
                transactionId: xml["transaction_id"]?.InnerText,
                outTradeNo: xml["out_trade_no"]?.InnerText,
                attach: xml["attach"]?.InnerText,
                timeEnd: xml["time_end"]?.InnerText
            );

            return await _paymentRecordRepository.UpdateAsync(record, true);
        }

        private static string JoinNodesInnerTextAsString(XmlNode xml, string prefix, int count, string separator = ",")
        {
            var innerTexts = new List<string>();
            
            for (var i = 0; i < count; i++)
            {
                var nodeName = prefix + i;

                var innerText = xml[nodeName]?.InnerText;

                if (innerText != null)
                {
                    innerTexts.Add(innerText);
                }
            }

            return innerTexts.JoinAsString(separator);
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