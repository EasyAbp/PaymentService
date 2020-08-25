using EasyAbp.PaymentService.Payments;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using EasyAbp.PaymentService.Refunds;

namespace EasyAbp.PaymentService.EntityFrameworkCore
{
    [ConnectionStringName(PaymentServiceDbProperties.ConnectionStringName)]
    public class PaymentServiceDbContext : AbpDbContext<PaymentServiceDbContext>, IPaymentServiceDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentItem> PaymentItems { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<RefundItem> RefundItems { get; set; }

        public PaymentServiceDbContext(DbContextOptions<PaymentServiceDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePaymentService();
        }
    }
}
