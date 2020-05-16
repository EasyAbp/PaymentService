using EasyAbp.PaymentService.WeChatPay.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.PaymentService.WeChatPay
{
    public abstract class WeChatPayController : AbpController
    {
        protected WeChatPayController()
        {
            LocalizationResource = typeof(WeChatPayResource);
        }
    }
}
