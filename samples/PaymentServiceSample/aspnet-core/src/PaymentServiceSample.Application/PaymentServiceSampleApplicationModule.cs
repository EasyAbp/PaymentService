using EasyAbp.PaymentService;
using EasyAbp.PaymentService.Prepayment;
using EasyAbp.PaymentService.WeChatPay;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;

namespace PaymentServiceSample
{
    [DependsOn(
        typeof(PaymentServiceSampleDomainModule),
        typeof(AbpAccountApplicationModule),
        typeof(PaymentServiceSampleApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(PaymentServiceApplicationModule),
        typeof(PaymentServicePrepaymentApplicationModule),
        typeof(PaymentServiceWeChatPayApplicationModule)
        )]
    public class PaymentServiceSampleApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<PaymentServiceSampleApplicationModule>();
            });
        }
    }
}
