using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EasyAbp.PaymentService.EntityFrameworkCore
{
    public class PaymentServiceModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public PaymentServiceModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}