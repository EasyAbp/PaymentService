using EasyAbp.PaymentService.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.PaymentService
{
    [ApiExplorerSettings(GroupName = "EasyAbpPaymentService")]
    public abstract class PaymentServiceController : AbpController
    {
        protected PaymentServiceController()
        {
            LocalizationResource = typeof(PaymentServiceResource);
        }
    }
}
