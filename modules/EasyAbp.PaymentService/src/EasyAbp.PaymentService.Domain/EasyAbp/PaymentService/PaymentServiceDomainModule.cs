using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.PaymentService
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpObjectExtendingModule),
        typeof(PaymentServiceDomainSharedModule)
        )]
    public class PaymentServiceDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.EtoMappings.Add<Payment, PaymentEto>(typeof(PaymentServiceDomainModule));
                
                options.AutoEventSelectors.Add<Payment>();
                options.AutoEventSelectors.Add<Refund>();
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<PaymentServiceDomainModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<PaymentServiceDomainAutoMapperProfile>(validate: true);
            });
        }
    }
}
