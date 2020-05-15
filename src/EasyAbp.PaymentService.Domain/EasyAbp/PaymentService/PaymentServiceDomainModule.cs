using EasyAbp.PaymentService.Payments;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(PaymentServiceDomainSharedModule)
        )]
    public class PaymentServiceDomainModule : AbpModule
    {
        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            var resolver = context.ServiceProvider.GetService<IPaymentServiceResolver>();

            if (resolver.GetPaymentMethods().Count == 0)
            {
                resolver.TryRegisterProvider(FreePaymentServiceProvider.PaymentMethod, typeof(FreePaymentServiceProvider));
            }
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<PaymentServiceDomainModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<PaymentServiceDomainAutoMapperProfile>(validate: true);
            });

            Configure<AbpDistributedEventBusOptions>(options =>
            {
                options.EtoMappings.Add<Payment, PaymentEto>(typeof(PaymentServiceDomainModule));
            });
        }
    }
}
