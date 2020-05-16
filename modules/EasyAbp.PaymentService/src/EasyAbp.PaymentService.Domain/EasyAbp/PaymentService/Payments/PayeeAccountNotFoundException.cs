using Volo.Abp;

namespace EasyAbp.PaymentService.Payments
{
    public class PayeeAccountNotFoundException : BusinessException
    {
        public PayeeAccountNotFoundException(string paymentMethod) : base(
            message: $"Cannot find the payee account of payment method {paymentMethod}.")
        {
        }
    }
}