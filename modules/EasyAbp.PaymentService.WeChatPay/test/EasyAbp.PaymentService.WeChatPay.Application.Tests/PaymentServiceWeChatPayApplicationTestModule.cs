using EasyAbp.PaymentService.WeChatPay;
using Volo.Abp.Modularity;

[DependsOn(
    typeof(PaymentServiceWeChatPayApplicationModule),
    typeof(PaymentServiceWeChatPayDomainTestModule)
)]
public class PaymentServiceWeChatPayApplicationTestModule : AbpModule
{

}