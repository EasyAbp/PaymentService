using PaymentServiceSample.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace PaymentServiceSample.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(PaymentServiceSampleEntityFrameworkCoreModule),
        typeof(PaymentServiceSampleApplicationContractsModule)
        )]
    public class PaymentServiceSampleDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
