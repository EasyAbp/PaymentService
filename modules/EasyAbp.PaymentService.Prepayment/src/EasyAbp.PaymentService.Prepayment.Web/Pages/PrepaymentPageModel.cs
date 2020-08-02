using EasyAbp.PaymentService.Prepayment.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EasyAbp.PaymentService.Prepayment.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class PrepaymentPageModel : AbpPageModel
    {
        protected PrepaymentPageModel()
        {
            LocalizationResourceType = typeof(PrepaymentResource);
            ObjectMapperContext = typeof(PaymentServicePrepaymentWebModule);
        }
    }
}