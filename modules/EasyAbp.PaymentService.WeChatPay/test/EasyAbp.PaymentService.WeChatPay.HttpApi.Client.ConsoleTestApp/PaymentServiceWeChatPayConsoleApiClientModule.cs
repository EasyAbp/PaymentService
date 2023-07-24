using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace EasyAbp.PaymentService.WeChatPay;

[DependsOn(
    typeof(PaymentServiceWeChatPayHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
)]
public class PaymentServiceWeChatPayConsoleApiClientModule : AbpModule
{
        
}