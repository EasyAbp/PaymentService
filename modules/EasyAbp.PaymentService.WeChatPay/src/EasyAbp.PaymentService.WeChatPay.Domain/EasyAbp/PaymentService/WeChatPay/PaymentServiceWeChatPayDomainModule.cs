using System.Collections.Generic;
using EasyAbp.Abp.WeChat.Pay;
using EasyAbp.Abp.WeChat.Pay.Infrastructure.OptionResolve;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.WeChatPay
{
    [DependsOn(
        typeof(PaymentServiceWeChatPayDomainSharedModule),
        typeof(AbpWeChatPayModule)
    )]
    public class PaymentServiceWeChatPayDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpWeChatPayResolveOptions>(options =>
            {
                options.Contributors.AddFirst(new SettingOptionsResolveContributor());
            });
        }
    }
}
