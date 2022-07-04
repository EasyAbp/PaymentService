using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore
{
    public static class WeChatPayDbContextModelCreatingExtensions
    {
        public static void ConfigurePaymentServiceWeChatPay(
            this ModelBuilder builder,
            Action<WeChatPayModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new WeChatPayModelBuilderConfigurationOptions(
                WeChatPayDbProperties.DbTablePrefix,
                WeChatPayDbProperties.DbSchema
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

            builder.Entity<RefundRecord>(b =>
            {
                b.ToTable(options.TablePrefix + "RefundRecords", options.Schema);
                b.ConfigureByConvention();

                /* Configure more properties here */
                b.HasIndex(x => x.PaymentId);
                b.HasIndex(x => x.OutRefundNo);
            });

            builder.Entity<PaymentRecord>(b =>
            {
                b.ToTable(options.TablePrefix + "PaymentRecords", options.Schema);
                b.ConfigureByConvention();

                /* Configure more properties here */
                b.HasIndex(x => x.PaymentId);
            });
        }
    }
}