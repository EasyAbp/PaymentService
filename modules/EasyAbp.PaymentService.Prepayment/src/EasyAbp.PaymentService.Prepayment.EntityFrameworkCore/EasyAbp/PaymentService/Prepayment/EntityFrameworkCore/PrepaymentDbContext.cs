using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using EasyAbp.PaymentService.Prepayment.Accounts;
using EasyAbp.PaymentService.Prepayment.Transactions;
using EasyAbp.PaymentService.Prepayment.WithdrawalRecords;
using EasyAbp.PaymentService.Prepayment.WithdrawalRequests;

namespace EasyAbp.PaymentService.Prepayment.EntityFrameworkCore
{
    [ConnectionStringName(PrepaymentDbProperties.ConnectionStringName)]
    public class PrepaymentDbContext : AbpDbContext<PrepaymentDbContext>, IPrepaymentDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<WithdrawalRecord> WithdrawalRecords { get; set; }
        public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }

        public PrepaymentDbContext(DbContextOptions<PrepaymentDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePaymentServicePrepayment();
        }
    }
}
