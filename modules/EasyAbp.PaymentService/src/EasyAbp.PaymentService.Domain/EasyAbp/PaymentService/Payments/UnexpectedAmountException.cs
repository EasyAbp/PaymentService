using Volo.Abp;

namespace EasyAbp.PaymentService.Payments
{
    public class UnexpectedAmountException : BusinessException
    {
        public UnexpectedAmountException(decimal amount) : base(message: $"The amount ({amount}) is not expected.")
        {
        }
    }
}