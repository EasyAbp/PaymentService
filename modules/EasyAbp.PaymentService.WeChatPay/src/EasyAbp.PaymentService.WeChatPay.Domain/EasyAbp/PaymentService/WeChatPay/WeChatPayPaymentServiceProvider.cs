using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay.Services.Pay;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using EasyAbp.PaymentService.WeChatPay.Settings;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class WeChatPayPaymentServiceProvider : IPaymentServiceProvider, ITransientDependency
    {
        private readonly ServiceProviderPayService _serviceProviderPayService;
        private readonly IConfiguration _configuration;
        private readonly ISettingProvider _settingProvider;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IPaymentOpenIdProvider _paymentOpenIdProvider;
        private readonly IPaymentRepository _paymentRepository;
        
        public const string PaymentMethod = "WeChatPay";
        
        public WeChatPayPaymentServiceProvider(
            ServiceProviderPayService serviceProviderPayService,
            IConfiguration configuration,
            ISettingProvider settingProvider,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            IPaymentRecordRepository paymentRecordRepository,
            IPaymentOpenIdProvider paymentOpenIdProvider,
            IPaymentRepository paymentRepository)
        {
            _serviceProviderPayService = serviceProviderPayService;
            _configuration = configuration;
            _settingProvider = settingProvider;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _paymentRecordRepository = paymentRecordRepository;
            _paymentOpenIdProvider = paymentOpenIdProvider;
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment> PayAsync(Payment payment, Dictionary<string, object> payeeConfigurations)
        {
            if (payment.Currency != "CNY")
            {
                throw new CurrencyNotSupportedException(payment.PaymentMethod, payment.Currency);
            }

            var payeeAccount = payeeConfigurations.GetOrDefault("PayeeAccount") as string ??
                               await _settingProvider.GetOrNullAsync(WeChatPaySettings.MchId);
            
            payment.SetPayeeAccount(payeeAccount);

            var appId = payment.ExtraProperties.GetOrDefault("appid") as string;
            
            var openId = await _paymentOpenIdProvider.FindUserOpenIdAsync(appId, payment.UserId);
            
            var outTradeNo = payment.Id.ToString("N");

            var result = await _serviceProviderPayService.UnifiedOrderAsync(
                appId: appId,
                subAppId: null,
                mchId: payment.PayeeAccount,
                subMchId: null,
                deviceInfo: payeeConfigurations.GetOrDefault("deviceInfo") as string ?? "EasyAbp Payment Service",
                body: payeeConfigurations.GetOrDefault("body") as string ?? "EasyAbp Payment Service",
                detail: payeeConfigurations.GetOrDefault("detail") as string,
                attach: payeeConfigurations.GetOrDefault("attach") as string,
                outTradeNo: outTradeNo,
                feeType: payment.Currency,
                totalFee: ConvertDecimalToWeChatPayFee(payment.ActualPaymentAmount),
                billCreateIp: "127.0.0.1",
                timeStart: null,
                timeExpire: null,
                goodsTag: payeeConfigurations.GetOrDefault("goods_tag") as string,
                notifyUrl: payeeConfigurations.GetOrDefault("notify_url") as string 
                           ?? _configuration["App:SelfUrl"].EnsureEndsWith('/') + "WeChatPay/Notify",
                tradeType: payment.ExtraProperties.GetOrDefault("trade_type") as string,
                productId: null,
                limitPay: payeeConfigurations.GetOrDefault("limit_pay") as string,
                openId: openId,
                subOpenId: null,
                receipt: payeeConfigurations.GetOrDefault("receipt") as string ?? "N",
                sceneInfo: null);

            var xml = result.SelectSingleNode("xml") ?? throw new UnifiedOrderFailedException();
            
            if (xml.SelectSingleNode("return_code")?.InnerText != "SUCCESS")
            {
                throw new UnifiedOrderFailedException(xml.SelectSingleNode("return_code")?.InnerText, xml.SelectSingleNode("return_msg")?.InnerText);
            }

            if (xml.SelectSingleNode("result_code")?.InnerText != "SUCCESS")
            {
                throw new UnifiedOrderFailedException(xml.SelectSingleNode("return_code")?.InnerText,
                    xml.SelectSingleNode("return_msg")?.InnerText, xml.SelectSingleNode("err_code_des")?.InnerText,
                    xml.SelectSingleNode("err_code")?.InnerText);
            }

            payment.SetProperty("trade_type", xml.SelectSingleNode("trade_type")?.InnerText);
            payment.SetProperty("prepay_id", xml.SelectSingleNode("prepay_id")?.InnerText);
            payment.SetProperty("code_url", xml.SelectSingleNode("code_url")?.InnerText);
            
            await _paymentRecordRepository.InsertAsync(
                new PaymentRecord(_guidGenerator.Create(), _currentTenant.Id, payment.Id));
            
            return await _paymentRepository.UpdateAsync(payment, true);
        }

        private static int ConvertDecimalToWeChatPayFee(decimal paymentActualPaymentAmount)
        {
            return Convert.ToInt32(decimal.Round(paymentActualPaymentAmount, 2, MidpointRounding.AwayFromZero) * 100);
        }
    }
}