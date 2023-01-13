using EasyAbp.PaymentService.WeChatPay.ObjectExtending;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace EasyAbp.PaymentService.WeChatPay
{
    [DependsOn(
        typeof(PaymentServiceWeChatPayDomainModule),
        typeof(PaymentServiceWeChatPayApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
    )]
    public class PaymentServiceWeChatPayApplicationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PaymentServiceWeChatPayApplicationObjectExtensions.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<PaymentServiceWeChatPayApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<PaymentServiceWeChatPayApplicationModule>(validate: false); // todo: https://github.com/abpframework/abp/issues/15404
            });
        }
    }
}
