using EasyAbp.PaymentService.Prepayment.ObjectExtending;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.Prepayment
{
    [DependsOn(
        typeof(PaymentServicePrepaymentDomainSharedModule),
        typeof(PaymentServiceDomainModule)
    )]
    public class PaymentServicePrepaymentDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PaymentServicePrepaymentDomainObjectExtensions.Configure();
        }
    }
}
