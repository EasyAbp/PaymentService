using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EasyAbp.PaymentService.Prepayment.MongoDB
{
    [ConnectionStringName(PrepaymentDbProperties.ConnectionStringName)]
    public interface IPrepaymentMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
