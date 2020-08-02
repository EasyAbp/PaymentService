using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace EasyAbp.PaymentService.Prepayment.MongoDB
{
    public class PrepaymentMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public PrepaymentMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}