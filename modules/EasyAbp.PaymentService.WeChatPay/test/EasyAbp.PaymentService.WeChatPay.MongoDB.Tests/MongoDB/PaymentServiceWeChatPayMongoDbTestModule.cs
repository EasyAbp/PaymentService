using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.WeChatPay.MongoDB
{
    [DependsOn(
        typeof(PaymentServiceWeChatPayTestBaseModule),
        typeof(PaymentServiceWeChatPayMongoDbModule)
        )]
    public class PaymentServiceWeChatPayMongoDbTestModule : AbpModule
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