using EasyAbp.PaymentService.Prepayment.WithdrawalRequests;
using EasyAbp.PaymentService.Prepayment.WithdrawalRecords;
using EasyAbp.PaymentService.Prepayment.Transactions;
using EasyAbp.PaymentService.Prepayment.Accounts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using WithdrawalRequests;

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
            context.Services.AddAbpDbContext<PaymentServicePrepaymentDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<Account, AccountRepository>();
                options.AddRepository<Transaction, TransactionRepository>();
                options.AddRepository<WithdrawalRecord, WithdrawalRecordRepository>();
                options.AddRepository<WithdrawalRequest, WithdrawalRequestRepository>();
            });
        }
    }
}
