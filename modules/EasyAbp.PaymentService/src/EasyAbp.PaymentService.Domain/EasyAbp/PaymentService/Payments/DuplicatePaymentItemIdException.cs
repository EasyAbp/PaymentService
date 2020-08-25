using Volo.Abp;

namespace EasyAbp.PaymentService.Payments
{
    public class DuplicatePaymentItemIdException : BusinessException
    {
        public DuplicatePaymentItemIdException() : base("DuplicatePaymentItemId", "The PaymentItemId must be unique.")
        {
        }
    }
}