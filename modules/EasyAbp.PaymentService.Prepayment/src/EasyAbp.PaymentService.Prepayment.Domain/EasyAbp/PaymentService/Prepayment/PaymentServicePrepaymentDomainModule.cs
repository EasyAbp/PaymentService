using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.Prepayment
{
    [DependsOn(
        typeof(PaymentServicePrepaymentDomainSharedModule),
        typeof(PaymentServiceDomainModule)
    )]
    public class PaymentServicePrepaymentDomainModule : AbpModule
    {

    }
}
