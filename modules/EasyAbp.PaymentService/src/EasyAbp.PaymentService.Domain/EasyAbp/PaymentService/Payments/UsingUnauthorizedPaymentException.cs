using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Payments
{
    public class UsingUnauthorizedPaymentException : BusinessException
    {
        public UsingUnauthorizedPaymentException(Guid userId, Guid paymentId) : base(
            message: $"The user ({userId}) is trying to use the payment ({paymentId}) that is not his own.")
        {
        }
    }
}