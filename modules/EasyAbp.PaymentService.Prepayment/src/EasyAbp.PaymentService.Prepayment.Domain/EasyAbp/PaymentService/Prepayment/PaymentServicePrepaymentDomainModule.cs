using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.Prepayment
{
    [DependsOn(
        typeof(PaymentServicePrepaymentDomainSharedModule)
        )]
    public class PaymentServicePrepaymentDomainModule : AbpModule
    {

    }
}
