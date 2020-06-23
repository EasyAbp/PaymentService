using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Refunds
{
    public class RefundIsInUnexpectedStageException : BusinessException
    {
        public RefundIsInUnexpectedStageException(Guid id) : base(message: $"Refund ({id}) is in unexpected stage.")
        {
        }
    }
}