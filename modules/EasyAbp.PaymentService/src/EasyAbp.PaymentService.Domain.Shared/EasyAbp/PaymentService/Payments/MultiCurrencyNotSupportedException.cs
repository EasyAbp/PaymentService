using Volo.Abp;

namespace EasyAbp.PaymentService.Payments
{
    public class MultiCurrencyNotSupportedException : BusinessException
    {
        public MultiCurrencyNotSupportedException() : base(message: $"Multi-currency is not supported.")
        {
        }
    }
}