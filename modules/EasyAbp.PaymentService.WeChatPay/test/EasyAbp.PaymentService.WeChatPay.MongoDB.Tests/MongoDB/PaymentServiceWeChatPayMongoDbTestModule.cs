using System;
using EasyAbp.PaymentService.WeChatPay.MongoDB;
using Mongo2Go;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace MongoDB
{
    [DependsOn(
        typeof(PaymentServiceWeChatPayTestBaseModule),
        typeof(PaymentServiceWeChatPayMongoDbModule)
        )]
    public class PaymentServiceWeChatPayMongoDbTestModule : AbpModule
    {
        private static readonly MongoDbRunner MongoDbRunner = MongoDbRunner.Start();

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var connectionString = MongoDbRunner.ConnectionString.EnsureEndsWith('/') +
                                   "Db_" +
                                    Guid.NewGuid().ToString("N");

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });
        }
    }
}