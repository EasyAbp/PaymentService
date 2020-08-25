using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Payments
{
    public class AnotherRefundIsInProgressException : BusinessException
    {
        public AnotherRefundIsInProgressException(Guid paymentId) : base("AnotherRefundIsInProgress",
            $"There is another refund with the same payment ({paymentId}) in progress.")
        {
        }
    }
}