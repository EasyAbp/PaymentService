using EasyAbp.PaymentService.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.PaymentService
{
    [Area(PaymentServiceRemoteServiceConsts.ModuleName)]
    [ApiExplorerSettings(GroupName = "EasyAbpPaymentService")]
    public abstract class PaymentServiceController : AbpControllerBase
    {
        protected PaymentServiceController()
        {
            LocalizationResource = typeof(PaymentServiceResource);
        }
    }
}
