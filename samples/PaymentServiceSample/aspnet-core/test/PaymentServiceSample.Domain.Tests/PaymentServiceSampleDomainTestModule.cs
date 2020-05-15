using PaymentServiceSample.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace PaymentServiceSample
{
    [DependsOn(
        typeof(PaymentServiceSampleEntityFrameworkCoreTestModule)
        )]
    public class PaymentServiceSampleDomainTestModule : AbpModule
    {

    }
}