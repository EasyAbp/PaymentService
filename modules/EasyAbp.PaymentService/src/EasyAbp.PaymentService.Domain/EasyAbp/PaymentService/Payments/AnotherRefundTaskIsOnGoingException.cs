using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Payments
{
    public class AnotherRefundTaskIsOnGoingException : BusinessException
    {
        public AnotherRefundTaskIsOnGoingException(Guid id)
            : base(message: $"Payment ({id}) has another ongoing refund task.")
        {
        }
    }
}