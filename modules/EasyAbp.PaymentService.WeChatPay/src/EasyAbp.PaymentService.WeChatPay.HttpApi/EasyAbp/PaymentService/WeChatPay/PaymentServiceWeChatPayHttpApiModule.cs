using EasyAbp.Abp.WeChat.Pay.HttpApi;
using Localization.Resources.AbpUi;
using EasyAbp.PaymentService.WeChatPay.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace EasyAbp.PaymentService.WeChatPay
{
    [DependsOn(
        typeof(PaymentServiceWeChatPayApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpWeChatPayHttpApiModule)
    )]
    public class PaymentServiceWeChatPayHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(PaymentServiceWeChatPayHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<WeChatPayResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
