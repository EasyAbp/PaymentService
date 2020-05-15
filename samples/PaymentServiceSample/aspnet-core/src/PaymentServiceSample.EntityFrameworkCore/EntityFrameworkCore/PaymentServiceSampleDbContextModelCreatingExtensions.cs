using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace PaymentServiceSample.EntityFrameworkCore
{
    public static class PaymentServiceSampleDbContextModelCreatingExtensions
    {
        public static void ConfigurePaymentServiceSample(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(PaymentServiceSampleConsts.DbTablePrefix + "YourEntities", PaymentServiceSampleConsts.DbSchema);

            //    //...
            //});
        }
    }
}