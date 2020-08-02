using EasyAbp.PaymentService.Prepayment.Localization;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.Prepayment
{
    public abstract class PrepaymentAppService : ApplicationService
    {
        protected PrepaymentAppService()
        {
            LocalizationResource = typeof(PrepaymentResource);
            ObjectMapperContext = typeof(PaymentServicePrepaymentApplicationModule);
        }
    }
}
