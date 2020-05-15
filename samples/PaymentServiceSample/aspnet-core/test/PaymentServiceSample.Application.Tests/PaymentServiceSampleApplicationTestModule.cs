using Volo.Abp.Modularity;

namespace PaymentServiceSample
{
    [DependsOn(
        typeof(PaymentServiceSampleApplicationModule),
        typeof(PaymentServiceSampleDomainTestModule)
        )]
    public class PaymentServiceSampleApplicationTestModule : AbpModule
    {

    }
}