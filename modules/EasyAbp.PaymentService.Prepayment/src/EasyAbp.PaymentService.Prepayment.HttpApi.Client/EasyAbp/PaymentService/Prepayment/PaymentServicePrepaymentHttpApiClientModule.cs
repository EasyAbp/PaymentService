using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.Prepayment
{
    [DependsOn(
        typeof(PaymentServicePrepaymentApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class PaymentServicePrepaymentHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "EasyAbpPaymentServicePrepayment";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(PaymentServicePrepaymentApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
