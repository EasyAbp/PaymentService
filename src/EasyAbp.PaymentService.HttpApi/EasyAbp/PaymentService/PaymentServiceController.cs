using EasyAbp.PaymentService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.PaymentService
{
    public abstract class PaymentServiceController : AbpController
    {
        protected PaymentServiceController()
        {
            LocalizationResource = typeof(PaymentServiceResource);
        }
    }
}
