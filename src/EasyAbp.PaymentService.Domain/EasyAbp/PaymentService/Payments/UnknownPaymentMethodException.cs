using Volo.Abp;

namespace EasyAbp.PaymentService.Payments
{
    public class UnknownPaymentMethodException : BusinessException
    {
        public UnknownPaymentMethodException(string paymentMethod) : base(
            message: $"Payment method {paymentMethod} does not exist.")
        {
        }
    }
}