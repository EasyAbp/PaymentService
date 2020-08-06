using EasyAbp.PaymentService.Prepayment.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.PaymentService.Prepayment
{
    [ApiExplorerSettings(GroupName = "EasyAbpPaymentServicePrepayment")]
    public abstract class PrepaymentController : AbpController
    {
        protected PrepaymentController()
        {
            LocalizationResource = typeof(PrepaymentResource);
        }
    }
}
