using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRecords
{
    public class WithdrawalIsNotInProgressException : BusinessException
    {
        public WithdrawalIsNotInProgressException() : base("WithdrawalIsNotInProgress",
            "The withdrawal is not in progress.")
        {
        }
    }
}