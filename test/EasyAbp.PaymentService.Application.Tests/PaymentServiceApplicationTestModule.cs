using EasyAbp.PaymentService;
using Volo.Abp.Modularity;

[DependsOn(
    typeof(PaymentServiceApplicationModule),
    typeof(PaymentServiceDomainTestModule)
)]
public class PaymentServiceApplicationTestModule : AbpModule
{

}