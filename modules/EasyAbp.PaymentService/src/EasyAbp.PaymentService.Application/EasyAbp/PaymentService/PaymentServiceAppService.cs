using EasyAbp.PaymentService.Localization;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService
{
    public abstract class PaymentServiceAppService : ApplicationService
    {
        protected PaymentServiceAppService()
        {
            LocalizationResource = typeof(PaymentServiceResource);
            ObjectMapperContext = typeof(PaymentServiceApplicationModule);
        }
    }
}
