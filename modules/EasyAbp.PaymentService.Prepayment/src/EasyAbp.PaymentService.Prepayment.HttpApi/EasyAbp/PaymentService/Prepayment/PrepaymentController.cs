using EasyAbp.PaymentService.Prepayment.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.PaymentService.Prepayment
{
    public abstract class PrepaymentController : AbpController
    {
        protected PrepaymentController()
        {
            LocalizationResource = typeof(PrepaymentResource);
        }
    }
}
