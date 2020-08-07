using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.PaymentService
{
    public class SelfTopUpException : BusinessException
    {
        public SelfTopUpException() : base(message: $"An account cannot top up itself.")
        {
        }
    }
}