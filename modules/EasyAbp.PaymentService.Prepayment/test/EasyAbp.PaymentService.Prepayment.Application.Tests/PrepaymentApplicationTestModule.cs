using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.Prepayment
{
    [DependsOn(
        typeof(PaymentServiceApplicationModule),
        typeof(PaymentServicePrepaymentApplicationModule),
        typeof(PrepaymentDomainTestModule)
    )]
    public class PrepaymentApplicationTestModule : AbpModule
    {

    }
}
