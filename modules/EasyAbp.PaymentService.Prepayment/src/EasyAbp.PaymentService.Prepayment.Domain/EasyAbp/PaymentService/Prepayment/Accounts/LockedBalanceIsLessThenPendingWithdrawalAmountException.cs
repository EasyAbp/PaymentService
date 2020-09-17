using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class LockedBalanceIsLessThenPendingWithdrawalAmountException : BusinessException
    {
        public LockedBalanceIsLessThenPendingWithdrawalAmountException(decimal lockedBalance, decimal withdrawalAmount)
            : base("LockedBalanceIsLessThenPendingWithdrawalAmount",
                $"The locked balance ({lockedBalance}) should be equal or greater than the pending withdrawal amount ({withdrawalAmount})")
        {
        }
    }
}