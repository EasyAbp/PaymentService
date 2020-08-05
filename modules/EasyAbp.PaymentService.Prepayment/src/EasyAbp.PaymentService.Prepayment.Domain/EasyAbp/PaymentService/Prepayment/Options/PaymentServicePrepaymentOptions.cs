using EasyAbp.PaymentService.Prepayment.Options.AccountGroups;
using EasyAbp.PaymentService.Prepayment.Options.WithdrawalMethods;

namespace EasyAbp.PaymentService.Prepayment.Options
{
    public class PaymentServicePrepaymentOptions
    {
        public AccountGroupConfigurations AccountGroups { get; }
        
        public WithdrawalMethodConfigurations WithdrawalMethods { get; }

        public PaymentServicePrepaymentOptions()
        {
            AccountGroups = new AccountGroupConfigurations();
            WithdrawalMethods = new WithdrawalMethodConfigurations();
        }
    }
}