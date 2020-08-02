using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace EasyAbp.PaymentService.Prepayment.MongoDB
{
    public static class PrepaymentMongoDbContextExtensions
    {
        public static void ConfigurePrepayment(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new PrepaymentMongoModelBuilderConfigurationOptions(
                PrepaymentDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}