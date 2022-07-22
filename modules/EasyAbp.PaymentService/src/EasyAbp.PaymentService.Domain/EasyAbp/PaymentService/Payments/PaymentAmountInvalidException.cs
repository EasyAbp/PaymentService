using Volo.Abp;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentAmountInvalidException : BusinessException
    {
        public PaymentAmountInvalidException(decimal paymentAmount) : base(
            message: $"Payment amount {paymentAmount} is invalid.")
        {
        }
        
        public PaymentAmountInvalidException(decimal paymentAmount, string paymentMethod) : base(
            message: $"Payment amount {paymentAmount} is invalid for payment method: {paymentMethod}.")
        {
        }
    }
}