using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using EasyAbp.PaymentService.Prepayment.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.PaymentService.Prepayment
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class PaymentServicePrepaymentDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<PaymentServicePrepaymentDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<PrepaymentResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/EasyAbp/PaymentService/Prepayment/Localization");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("EasyAbp.PaymentService.Prepayment", typeof(PrepaymentResource));
            });
        }
    }
}
