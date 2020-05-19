using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore
{
    [DependsOn(
        typeof(PaymentServiceWeChatPayDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class PaymentServiceWeChatPayEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<WeChatPayDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<RefundRecord, RefundRecordRepository>();
                options.AddRepository<PaymentRecord, PaymentRecordRepository>();
            });
        }
    }
}
