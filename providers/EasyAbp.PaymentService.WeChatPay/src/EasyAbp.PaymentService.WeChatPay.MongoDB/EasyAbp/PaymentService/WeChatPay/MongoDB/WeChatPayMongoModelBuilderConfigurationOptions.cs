using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace EasyAbp.PaymentService.WeChatPay.MongoDB
{
    public class WeChatPayMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public WeChatPayMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}