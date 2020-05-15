using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace PaymentServiceSample.EntityFrameworkCore
{
    [DependsOn(
        typeof(PaymentServiceSampleEntityFrameworkCoreModule)
        )]
    public class PaymentServiceSampleEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PaymentServiceSampleMigrationsDbContext>();
        }
    }
}
