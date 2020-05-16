using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using EasyAbp.PaymentService.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EasyAbp.PaymentService.Web.Pages
{
    /* Inherit your UI Pages from this class. To do that, add this line to your Pages (.cshtml files under the Page folder):
     * @inherits EasyAbp.PaymentService.Web.Pages.PaymentServicePage
     */
    public abstract class PaymentServicePage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<PaymentServiceResource> L { get; set; }
    }
}
