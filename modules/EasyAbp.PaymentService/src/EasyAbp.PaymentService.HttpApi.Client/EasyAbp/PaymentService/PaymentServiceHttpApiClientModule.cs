using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService
{
    [DependsOn(
        typeof(PaymentServiceApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class PaymentServiceHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "EasyAbpPaymentService";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(PaymentServiceApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
