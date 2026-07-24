using EasyAbp.PaymentService.WeChatPay.ObjectExtending;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace EasyAbp.PaymentService.WeChatPay
{
    [DependsOn(
        typeof(PaymentServiceWeChatPayDomainModule),
        typeof(PaymentServiceWeChatPayApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpMapperlyModule)
    )]
    public class PaymentServiceWeChatPayApplicationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PaymentServiceWeChatPayApplicationObjectExtensions.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMapperlyObjectMapper<PaymentServiceWeChatPayApplicationModule>();
        }
    }
}
