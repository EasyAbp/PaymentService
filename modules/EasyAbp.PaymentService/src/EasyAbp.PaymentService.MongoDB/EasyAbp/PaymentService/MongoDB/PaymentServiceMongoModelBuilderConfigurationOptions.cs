using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace EasyAbp.PaymentService.MongoDB
{
    public class PaymentServiceMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public PaymentServiceMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}