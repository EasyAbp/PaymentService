using EasyAbp.PaymentService.Options;
using EasyAbp.PaymentService.Prepayment.Options;
using EasyAbp.PaymentService.Prepayment.PaymentService;
using EasyAbp.PaymentService.Prepayment.WithdrawalRequests;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace EasyAbp.PaymentService.Prepayment
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAuthorizationModule),
        typeof(PaymentServicePrepaymentDomainModule)
        )]
    public class PrepaymentTestBaseModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysAllowAuthorization();

            Configure<PaymentServiceOptions>(options =>
            {
                options.Providers.Configure<PrepaymentPaymentServiceProvider>(PrepaymentPaymentServiceProvider.PaymentMethod);
            });
            
            Configure<PaymentServicePrepaymentOptions>(options =>
            {
                options.AccountGroups.Configure<DefaultAccountGroup>(accountGroup =>
                {
                    accountGroup.Currency = "CNY";
                });
                
                options.AccountGroups.Configure<CustomAccountGroup>(accountGroup =>
                {
                    accountGroup.Currency = "CNY";
                    accountGroup.AllowedUsingToTopUpOtherAccounts = true;
                });
                
                options.WithdrawalMethods.Configure<ManualWithdrawalMethod>(withdrawalMethod =>
                {
                    withdrawalMethod.AccountWithdrawalProviderType = typeof(ManualAccountWithdrawalProvider);
                    withdrawalMethod.DailyMaximumWithdrawalAmountEachAccount = 5m;
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            AsyncHelper.RunSync(async () =>
            {
                using (var scope = context.ServiceProvider.CreateScope())
                {
                    await scope.ServiceProvider
                        .GetRequiredService<IDataSeeder>()
                        .SeedAsync();
                }
            });
        }
    }
}
