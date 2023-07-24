using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.WeChatPay;

[DependsOn(
    typeof(PaymentServiceApplicationModule),
    typeof(PaymentServiceWeChatPayApplicationModule),
    typeof(PaymentServiceWeChatPayDomainTestModule)
)]
public class PaymentServiceWeChatPayApplicationTestModule : AbpModule
{

}