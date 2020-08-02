using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EasyAbp.PaymentService.Prepayment.EntityFrameworkCore
{
    public class PrepaymentModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public PrepaymentModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}