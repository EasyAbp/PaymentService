using EasyAbp.PaymentService;
using EasyAbp.PaymentService.WeChatPay;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;

namespace PaymentServiceSample
{
    [DependsOn(
        typeof(PaymentServiceSampleApplicationContractsModule),
        typeof(AbpAccountHttpApiClientModule),
        typeof(AbpIdentityHttpApiClientModule),
        typeof(AbpPermissionManagementHttpApiClientModule),
        typeof(AbpTenantManagementHttpApiClientModule),
        typeof(AbpFeatureManagementHttpApiClientModule),
        typeof(PaymentServiceHttpApiClientModule),
        typeof(PaymentServiceWeChatPayHttpApiClientModule)
    )]
    public class PaymentServiceSampleHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(PaymentServiceSampleApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
