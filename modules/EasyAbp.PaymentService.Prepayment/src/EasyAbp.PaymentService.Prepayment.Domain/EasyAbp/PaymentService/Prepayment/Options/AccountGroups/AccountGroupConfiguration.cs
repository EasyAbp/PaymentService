namespace EasyAbp.PaymentService.Prepayment.Options.AccountGroups
{
    public class AccountGroupConfiguration
    {
        /// <summary>
        /// Admin should manually create users' account.
        /// </summary>
        public bool DisableAccountAutoCreation { get; set; }
        
        /// <summary>
        /// Only admin can change accounts' balance.
        /// </summary>
        public bool DisableUserRecharge { get; set; }
        
        /// <summary>
        /// The account cannot be a payment method.
        /// </summary>
        public bool DisableUserPayWithAccount { get; set; }
        
        /// <summary>
        /// The balance cannot be withdrawn.
        /// </summary>
        public bool DisableUserWithdrawal { get; set; }
    }
}