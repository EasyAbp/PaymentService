using System;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class WithdrawalInProgressNotFoundException : BusinessException
    {
        public WithdrawalInProgressNotFoundException()
            : base("WithdrawalInProgressNotFound","The withdrawal in progress not found.")
        {
        }
    }
}