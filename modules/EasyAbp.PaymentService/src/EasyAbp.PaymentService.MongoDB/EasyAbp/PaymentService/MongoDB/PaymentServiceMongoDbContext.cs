using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EasyAbp.PaymentService.MongoDB
{
    [ConnectionStringName(PaymentServiceDbProperties.ConnectionStringName)]
    public class PaymentServiceMongoDbContext : AbpMongoDbContext, IPaymentServiceMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigurePaymentService();
        }
    }
}