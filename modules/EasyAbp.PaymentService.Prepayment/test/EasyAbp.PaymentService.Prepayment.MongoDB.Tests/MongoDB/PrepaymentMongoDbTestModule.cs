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
            var connectionString = MongoDbFixture.ConnectionString.EnsureEndsWith('/') +
                                   "Db_" +
                                    Guid.NewGuid().ToString("N");

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });
        }
    }
}