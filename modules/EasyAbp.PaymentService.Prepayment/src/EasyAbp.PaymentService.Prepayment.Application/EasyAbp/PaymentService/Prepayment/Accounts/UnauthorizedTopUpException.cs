using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class UnauthorizedTopUpException : BusinessException
    {
        public UnauthorizedTopUpException(Guid accountId)
            : base(message: $"Cannot top up the account ({accountId}).")
        {
        }
    }
}