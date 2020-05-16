using EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore;
using Volo.Abp.Modularity;

/* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
[DependsOn(
    typeof(PaymentServiceWeChatPayEntityFrameworkCoreTestModule)
)]
public class PaymentServiceWeChatPayDomainTestModule : AbpModule
{
        
}