using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using PaymentServiceSample.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace PaymentServiceSample.Web.Pages
{
    /* Inherit your UI Pages from this class. To do that, add this line to your Pages (.cshtml files under the Page folder):
     * @inherits PaymentServiceSample.Web.Pages.PaymentServiceSamplePage
     */
    public abstract class PaymentServiceSamplePage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<PaymentServiceSampleResource> L { get; set; }
    }
}
