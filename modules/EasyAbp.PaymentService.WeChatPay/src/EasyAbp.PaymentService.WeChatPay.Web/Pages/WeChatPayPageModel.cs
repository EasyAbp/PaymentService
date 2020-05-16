using EasyAbp.PaymentService.WeChatPay.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EasyAbp.PaymentService.WeChatPay.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class WeChatPayPageModel : AbpPageModel
    {
        protected WeChatPayPageModel()
        {
            LocalizationResourceType = typeof(WeChatPayResource);
            ObjectMapperContext = typeof(PaymentServiceWeChatPayWebModule);
        }
    }
}