using EasyAbp.Abp.WeChat.Pay;
using EasyAbp.PaymentService.WeChatPay.ObjectExtending;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.WeChatPay
{
    [DependsOn(
        typeof(PaymentServiceDomainModule),
        typeof(PaymentServiceWeChatPayDomainSharedModule),
        typeof(AbpBackgroundJobsAbstractionsModule),
        typeof(AbpWeChatPayModule)
    )]
    public class PaymentServiceWeChatPayDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PaymentServiceWeChatPayDomainObjectExtensions.Configure();
        }
    }
}