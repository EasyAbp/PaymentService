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
        }
    }
}