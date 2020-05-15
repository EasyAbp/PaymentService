using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace PaymentServiceSample.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(PaymentServiceSampleHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class PaymentServiceSampleConsoleApiClientModule : AbpModule
    {
        
    }
}
