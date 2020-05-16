using EasyAbp.PaymentService.WeChatPay.Localization;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.WeChatPay
{
    public abstract class WeChatPayAppService : ApplicationService
    {
        protected WeChatPayAppService()
        {
            LocalizationResource = typeof(WeChatPayResource);
            ObjectMapperContext = typeof(PaymentServiceWeChatPayApplicationModule);
        }
    }
}
