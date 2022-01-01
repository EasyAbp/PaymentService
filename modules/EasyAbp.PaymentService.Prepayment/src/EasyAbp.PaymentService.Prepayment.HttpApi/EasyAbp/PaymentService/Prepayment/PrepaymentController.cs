using EasyAbp.PaymentService.Prepayment.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.PaymentService.Prepayment
{
    [Area(PaymentServiceRemoteServiceConsts.ModuleName)]
    [ApiExplorerSettings(GroupName = "EasyAbpPaymentServicePrepayment")]
    public abstract class PrepaymentController : AbpControllerBase
    {
        protected PrepaymentController()
        {
            LocalizationResource = typeof(PrepaymentResource);
        }
    }
}
