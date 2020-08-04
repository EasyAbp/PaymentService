using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class UnauthorizedRechargeException : BusinessException
    {
        public UnauthorizedRechargeException(Guid accountId)
            : base(message: $"Cannot recharge the account ({accountId}).")
        {
        }
    }
}