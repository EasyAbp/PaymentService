using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class WithdrawalIsAlreadyInProgressException : BusinessException
    {
        public WithdrawalIsAlreadyInProgressException()
            : base("WithdrawalIsAlreadyInProgress","Another withdrawal for the account is already in progress.")
        {
        }
    }
}