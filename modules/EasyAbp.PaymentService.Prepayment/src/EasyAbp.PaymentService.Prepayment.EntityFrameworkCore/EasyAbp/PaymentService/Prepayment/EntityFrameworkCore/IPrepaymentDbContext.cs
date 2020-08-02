using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Prepayment.EntityFrameworkCore
{
    [ConnectionStringName(PrepaymentDbProperties.ConnectionStringName)]
    public interface IPrepaymentDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}