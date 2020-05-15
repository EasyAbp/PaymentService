using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.WeChatPay
{
    [DependsOn(
        typeof(PaymentServiceWeChatPayApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class PaymentServiceWeChatPayHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "WeChatPay";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(PaymentServiceWeChatPayApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
