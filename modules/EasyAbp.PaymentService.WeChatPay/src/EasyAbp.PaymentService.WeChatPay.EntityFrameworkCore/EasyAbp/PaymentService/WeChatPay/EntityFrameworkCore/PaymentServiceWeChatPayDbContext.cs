using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;

namespace EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore
{
    [ConnectionStringName(WeChatPayDbProperties.ConnectionStringName)]
    public class PaymentServiceWeChatPayDbContext : AbpDbContext<PaymentServiceWeChatPayDbContext>, IPaymentServiceWeChatPayDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */
        public DbSet<RefundRecord> RefundRecords { get; set; }
        public DbSet<PaymentRecord> PaymentRecords { get; set; }

        public PaymentServiceWeChatPayDbContext(DbContextOptions<PaymentServiceWeChatPayDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePaymentServiceWeChatPay();
        }
    }
}
