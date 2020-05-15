using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentHasAlreadyBeenCompletedException : BusinessException
    {
        public PaymentHasAlreadyBeenCompletedException(Guid id) : base(
            message: $"Payment ({id}) has already been completed.")
        {
        }
    }
}