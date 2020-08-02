using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace EasyAbp.PaymentService.Prepayment.MongoDB
{
    [DependsOn(
        typeof(PaymentServicePrepaymentDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class PaymentServicePrepaymentMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<PrepaymentMongoDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
            });
        }
    }
}
