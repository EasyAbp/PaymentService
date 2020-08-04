using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class LockedBalanceIsGreaterThenBalanceException : BusinessException
    {
        public LockedBalanceIsGreaterThenBalanceException(decimal lockedBalance, decimal balance)
            : base(message: $"The locked balance ({lockedBalance}) should be less than the current balance ({balance})")
        {
        }
    }
}