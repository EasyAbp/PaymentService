using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.PaymentService
{
    public class SelfRechargingException : BusinessException
    {
        public SelfRechargingException() : base(message: $"An account cannot recharge itself.")
        {
        }
    }
}