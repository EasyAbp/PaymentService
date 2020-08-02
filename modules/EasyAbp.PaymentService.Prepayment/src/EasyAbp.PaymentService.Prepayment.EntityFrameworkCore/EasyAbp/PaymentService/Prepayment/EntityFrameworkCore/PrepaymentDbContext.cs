using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Prepayment.EntityFrameworkCore
{
    [ConnectionStringName(PrepaymentDbProperties.ConnectionStringName)]
    public class PrepaymentDbContext : AbpDbContext<PrepaymentDbContext>, IPrepaymentDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public PrepaymentDbContext(DbContextOptions<PrepaymentDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePrepayment();
        }
    }
}