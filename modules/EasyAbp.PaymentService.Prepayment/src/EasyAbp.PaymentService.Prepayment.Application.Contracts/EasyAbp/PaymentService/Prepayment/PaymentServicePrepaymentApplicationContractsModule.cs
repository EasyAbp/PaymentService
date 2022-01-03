using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace EasyAbp.PaymentService.Prepayment
{
    [DependsOn(
        typeof(PaymentServicePrepaymentDomainSharedModule),
        typeof(PaymentServiceApplicationContractsModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class PaymentServicePrepaymentApplicationContractsModule : AbpModule
    {

    }
}
