using EasyAbp.PaymentService.WeChatPay;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

[DependsOn(
    typeof(PaymentServiceWeChatPayHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
)]
public class PaymentServiceWeChatPayConsoleApiClientModule : AbpModule
{
        
}