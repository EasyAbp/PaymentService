using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.EntityFrameworkCore
{
    [DependsOn(
        typeof(PaymentServiceDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class PaymentServiceEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PaymentServiceDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<Payment, PaymentRepository>();
                options.AddRepository<Refund, RefundRepository>();
            });
        }
    }
}
