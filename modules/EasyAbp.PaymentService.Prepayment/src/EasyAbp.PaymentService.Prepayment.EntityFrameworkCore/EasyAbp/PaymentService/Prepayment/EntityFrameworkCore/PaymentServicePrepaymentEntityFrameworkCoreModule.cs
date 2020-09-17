using EasyAbp.PaymentService.Prepayment.WithdrawalRecords;
using EasyAbp.PaymentService.Prepayment.Transactions;
using EasyAbp.PaymentService.Prepayment.Accounts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.Prepayment.EntityFrameworkCore
{
    [DependsOn(
        typeof(PaymentServicePrepaymentDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class PaymentServicePrepaymentEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PrepaymentDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<Account, AccountRepository>();
                options.AddRepository<Transaction, TransactionRepository>();
                options.AddRepository<WithdrawalRecord, WithdrawalRecordRepository>();
            });
        }
    }
}
