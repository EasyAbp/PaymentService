using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService;

[DependsOn(
    typeof(PaymentServiceApplicationModule),
    typeof(PaymentServiceDomainTestModule)
)]
public class PaymentServiceApplicationTestModule : AbpModule
{

}