using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.Prepayment
{
    [DependsOn(
        typeof(PaymentServicePrepaymentHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class PrepaymentConsoleApiClientModule : AbpModule
    {
        
    }
}
