using System;

namespace EasyAbp.PaymentService.Prepayment.Options.WithdrawalMethods
{
    public class WithdrawalMethodConfiguration
    {
        public Type AccountWithdrawalProviderType { get; set; }
        
        public decimal? DailyMaximumWithdrawalAmountEachAccount { get; set; }
    }
}