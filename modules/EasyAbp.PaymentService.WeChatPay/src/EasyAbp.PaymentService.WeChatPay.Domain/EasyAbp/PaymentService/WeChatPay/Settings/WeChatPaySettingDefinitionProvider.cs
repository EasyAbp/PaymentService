using System;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Settings;

namespace EasyAbp.PaymentService.WeChatPay.Settings
{
    public class WeChatPaySettingDefinitionProvider : SettingDefinitionProvider
    {
        private readonly IConfiguration _configuration;

        public WeChatPaySettingDefinitionProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public override void Define(ISettingDefinitionContext context)
        {
            /* Define module settings here.
             * Use names from WeChatPaySettings class.
             */
            
            context.Add(
                new SettingDefinition(WeChatPaySettings.MchId),
                new SettingDefinition(WeChatPaySettings.ApiKey),
                new SettingDefinition(WeChatPaySettings.IsSandBox, "false"),
                new SettingDefinition(WeChatPaySettings.NotifyUrl,
                    _configuration["App:SelfUrl"]?.EnsureEndsWith('/') + "wechat-pay/notify"),
                new SettingDefinition(WeChatPaySettings.RefundNotifyUrl,
                    _configuration["App:SelfUrl"]?.EnsureEndsWith('/') + "wechat-pay/refund-notify"),
                new SettingDefinition(WeChatPaySettings.CertificateBlobContainerName),
                new SettingDefinition(WeChatPaySettings.CertificateBlobName),
                new SettingDefinition(WeChatPaySettings.CertificateSecret)
            );
        }
    }
}