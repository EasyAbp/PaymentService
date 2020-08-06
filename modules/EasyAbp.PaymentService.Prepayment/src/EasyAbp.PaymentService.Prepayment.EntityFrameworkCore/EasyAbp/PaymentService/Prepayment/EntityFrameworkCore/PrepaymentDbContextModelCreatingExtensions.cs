using EasyAbp.PaymentService.Prepayment.Transactions;
using EasyAbp.PaymentService.Prepayment.Accounts;
using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EasyAbp.PaymentService.Prepayment.EntityFrameworkCore
{
    public static class PrepaymentDbContextModelCreatingExtensions
    {
        public static void ConfigurePaymentServicePrepayment(
            this ModelBuilder builder,
            Action<PrepaymentModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new PrepaymentModelBuilderConfigurationOptions(
                PrepaymentDbProperties.DbTablePrefix,
                PrepaymentDbProperties.DbSchema
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

            builder.Entity<Account>(b =>
            {
                b.ToTable(options.TablePrefix + "Accounts", options.Schema);
                b.ConfigureByConvention(); 
                
                /* Configure more properties here */
                b.Property(x => x.Balance).HasColumnType("decimal(20,8)");
                b.Property(x => x.LockedBalance).HasColumnType("decimal(20,8)");
                b.HasIndex(x => x.UserId);
            });

            builder.Entity<Transaction>(b =>
            {
                b.ToTable(options.TablePrefix + "Transactions", options.Schema);
                b.ConfigureByConvention();

                /* Configure more properties here */
                b.Property(x => x.ChangedBalance).HasColumnType("decimal(20,8)");
                b.Property(x => x.OriginalBalance).HasColumnType("decimal(20,8)");
                b.HasIndex(x => x.AccountId);
                b.HasIndex(x => x.AccountUserId);
            });
        }
    }
}
