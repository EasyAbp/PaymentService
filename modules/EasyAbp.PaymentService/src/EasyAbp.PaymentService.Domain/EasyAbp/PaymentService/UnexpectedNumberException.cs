using Volo.Abp;

namespace EasyAbp.PaymentService
{
    public class UnexpectedNumberException : BusinessException
    {
        public UnexpectedNumberException(decimal number) : base(message: $"The number ({number}) is not expected.")
        {
        }
    }
}