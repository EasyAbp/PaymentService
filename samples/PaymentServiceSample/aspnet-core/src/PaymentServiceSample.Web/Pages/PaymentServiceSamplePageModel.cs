using PaymentServiceSample.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace PaymentServiceSample.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class PaymentServiceSamplePageModel : AbpPageModel
    {
        protected PaymentServiceSamplePageModel()
        {
            LocalizationResourceType = typeof(PaymentServiceSampleResource);
        }
    }
}