using EasyAbp.PaymentService;
using EasyAbp.PaymentService.WeChatPay;
using Volo.Abp.Modularity;

[DependsOn(
    typeof(PaymentServiceApplicationModule),
    typeof(PaymentServiceWeChatPayApplicationModule),
    typeof(PaymentServiceWeChatPayDomainTestModule)
)]
public class PaymentServiceWeChatPayApplicationTestModule : AbpModule
{

}