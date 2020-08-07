using JetBrains.Annotations;

namespace EasyAbp.PaymentService.Prepayment.Options.AccountGroups
{
    public class AccountGroupConfiguration
    {
        /// <summary>
        /// The currency of the account balance, the default value is "USD".
        /// </summary>
        [NotNull]
        public string Currency { get; set; } = "USD";
        
        /// <summary>
        /// Admin should manually create users' account.
        /// </summary>
        public bool DisableAccountAutoCreation { get; set; }
        
        /// <summary>
        /// Only admin can change accounts' balance.
        /// </summary>
        public bool DisableUserTopUp { get; set; }
        
        /// <summary>
        /// The account cannot be a payment method.
        /// </summary>
        public bool DisableUserPayWithAccount { get; set; }
        
        /// <summary>
        /// The balance cannot be withdrawn.
        /// </summary>
        public bool DisableUserWithdrawal { get; set; }
        
        /// <summary>
        /// Can be the payment method to top up other prepayment accounts.
        /// </summary>
        public bool AllowedUsingToTopUpOtherAccounts { get; set; }
    }
}