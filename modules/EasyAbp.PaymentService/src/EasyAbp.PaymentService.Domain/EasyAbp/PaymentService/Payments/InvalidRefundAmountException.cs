using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Payments
{
    public class InvalidRefundAmountException : BusinessException
    {
        public InvalidRefundAmountException(Guid id, decimal refundAmount)
            : base(message: $"Refund amount ({refundAmount}) is invalid for the payment ({id}).")
        {
        }
    }
}