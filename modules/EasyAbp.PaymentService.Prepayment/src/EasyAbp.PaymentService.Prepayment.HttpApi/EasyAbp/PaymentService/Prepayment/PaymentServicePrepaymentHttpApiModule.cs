using Localization.Resources.AbpUi;
using EasyAbp.PaymentService.Prepayment.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace EasyAbp.PaymentService.Prepayment
{
    [DependsOn(
        typeof(PaymentServicePrepaymentApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class PaymentServicePrepaymentHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(PaymentServicePrepaymentHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PrepaymentResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
