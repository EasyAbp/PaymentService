using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EasyAbp.PaymentService.MongoDB
{
    [ConnectionStringName(PaymentServiceDbProperties.ConnectionStringName)]
    public interface IPaymentServiceMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
