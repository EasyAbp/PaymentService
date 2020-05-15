using EasyAbp.PaymentService.Refunds;
using System;
using EasyAbp.PaymentService.Payments;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EasyAbp.PaymentService.EntityFrameworkCore
{
    public static class PaymentServiceDbContextModelCreatingExtensions
    {
        public static void ConfigurePaymentService(
            this ModelBuilder builder,
            Action<PaymentServiceModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new PaymentServiceModelBuilderConfigurationOptions(
                PaymentServiceDbProperties.DbTablePrefix,
                PaymentServiceDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Questions", options.Schema);
            
                b.ConfigureByConvention();
            
                //Properties
                b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);
                
                //Relations
                b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                //Indexes
                b.HasIndex(q => q.CreationTime);
            });
            */

            builder.Entity<Payment>(b =>
            {
                b.ToTable(options.TablePrefix + "PaymentService", options.Schema);
                b.ConfigureByConvention();
                /* Configure more properties here */
                b.Property(x => x.ActualPaymentAmount).HasColumnType("decimal(18,6)");
                b.Property(x => x.OriginalPaymentAmount).HasColumnType("decimal(18,6)");
                b.Property(x => x.PaymentDiscount).HasColumnType("decimal(18,6)");
                b.Property(x => x.RefundAmount).HasColumnType("decimal(18,6)");
            });

            builder.Entity<Refund>(b =>
            {
                b.ToTable(options.TablePrefix + "Refunds", options.Schema);
                b.ConfigureByConvention(); 
                /* Configure more properties here */
                b.Property(x => x.RefundAmount).HasColumnType("decimal(18,6)");
            });

            builder.Entity<PaymentItem>(b =>
            {
                b.ToTable(options.TablePrefix + "PaymentItems", options.Schema);
                b.ConfigureByConvention(); 
                /* Configure more properties here */
                b.Property(x => x.ActualPaymentAmount).HasColumnType("decimal(18,6)");
                b.Property(x => x.OriginalPaymentAmount).HasColumnType("decimal(18,6)");
                b.Property(x => x.PaymentDiscount).HasColumnType("decimal(18,6)");
                b.Property(x => x.RefundAmount).HasColumnType("decimal(18,6)");
            });
        }
    }
}
