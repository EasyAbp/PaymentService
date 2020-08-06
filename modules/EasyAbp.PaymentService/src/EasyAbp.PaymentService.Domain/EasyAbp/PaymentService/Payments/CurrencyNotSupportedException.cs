using Volo.Abp;

namespace EasyAbp.PaymentService.Payments
{
    public class CurrencyNotSupportedException : BusinessException
    {
        public CurrencyNotSupportedException(string paymentMethod, string currency) : base(
            message: $"Payment method {paymentMethod} does not support currency: {currency}.")
        {
        }
        
        public CurrencyNotSupportedException(string currency) : base(
            message: $"The currency: {currency} is not supported.")
        {
        }
    }
}