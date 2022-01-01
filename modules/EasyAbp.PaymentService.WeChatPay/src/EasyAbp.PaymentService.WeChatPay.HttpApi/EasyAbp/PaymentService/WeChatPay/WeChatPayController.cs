using EasyAbp.PaymentService.WeChatPay.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.PaymentService.WeChatPay
{
    [Area(PaymentServiceRemoteServiceConsts.ModuleName)]
    [ApiExplorerSettings(GroupName = "EasyAbpPaymentServiceWeChatPay")]
    public abstract class WeChatPayController : AbpControllerBase
    {
        protected WeChatPayController()
        {
            LocalizationResource = typeof(WeChatPayResource);
        }
    }
}
