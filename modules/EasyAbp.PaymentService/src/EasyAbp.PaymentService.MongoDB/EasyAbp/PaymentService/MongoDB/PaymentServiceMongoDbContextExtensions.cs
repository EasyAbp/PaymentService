using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace EasyAbp.PaymentService.MongoDB
{
    public static class PaymentServiceMongoDbContextExtensions
    {
        public static void ConfigurePaymentService(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new PaymentServiceMongoModelBuilderConfigurationOptions(
                PaymentServiceDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}