using EasyAbp.PaymentService.WeChatPay.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.PaymentService.WeChatPay
{
    [ApiExplorerSettings(GroupName = "EasyAbpPaymentServiceWeChatPay")]
    public abstract class WeChatPayController : AbpController
    {
        protected WeChatPayController()
        {
            LocalizationResource = typeof(WeChatPayResource);
        }
    }
}
