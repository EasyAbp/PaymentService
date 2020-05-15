using PaymentServiceSample.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PaymentServiceSample.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class PaymentServiceSampleController : AbpController
    {
        protected PaymentServiceSampleController()
        {
            LocalizationResource = typeof(PaymentServiceSampleResource);
        }
    }
}