using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.Prepayment.MongoDB
{
    [DependsOn(
        typeof(PrepaymentTestBaseModule),
        typeof(PaymentServicePrepaymentMongoDbModule)
        )]
    public class PrepaymentMongoDbTestModule : AbpModule
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