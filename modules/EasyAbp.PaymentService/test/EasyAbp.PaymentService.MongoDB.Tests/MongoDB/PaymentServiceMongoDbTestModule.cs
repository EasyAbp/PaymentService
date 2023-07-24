using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.MongoDB
{
    [DependsOn(
        typeof(PaymentServiceTestBaseModule),
        typeof(PaymentServiceMongoDbModule)
        )]
    public class PaymentServiceMongoDbTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
            });
        }
    }
}