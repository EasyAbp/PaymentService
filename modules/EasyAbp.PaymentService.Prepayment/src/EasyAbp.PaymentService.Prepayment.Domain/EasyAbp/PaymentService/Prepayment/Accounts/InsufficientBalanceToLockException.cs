using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class InsufficientBalanceToLockException : BusinessException
    {
        public InsufficientBalanceToLockException(decimal lockedBalance, decimal balance) : base(
            message:
            $"Failed to lock {lockedBalance} balance. The current balance ({balance}) is insufficient to lock.")
        {
        }
    }
}