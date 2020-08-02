using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EasyAbp.PaymentService.Prepayment.MongoDB
{
    [ConnectionStringName(PrepaymentDbProperties.ConnectionStringName)]
    public class PrepaymentMongoDbContext : AbpMongoDbContext, IPrepaymentMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigurePrepayment();
        }
    }
}