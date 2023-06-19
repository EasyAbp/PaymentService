namespace EasyAbp.PaymentService.Prepayment
{
    public static class PrepaymentConsts
    {
        public const decimal AccountMinBalance = -999999999999.99999999m;
        
        public const decimal AccountMaxBalance = 999999999999.99999999m;
        
        public const decimal AccountMinLockedBalance = decimal.Zero;
        
        public const decimal AccountMaxLockedBalance = AccountMaxBalance;

        public const string ManualOperationPaymentMethod = "ManualOperation";

        public const string ChangeBalanceActionName = "ChangeBalance";

        public const string ChangeBalancePaymentMethod = ManualOperationPaymentMethod;
        
        public const string PaymentActionName = "Payment";
        
        public const string RefundActionName = "Refund";

        public const string TopUpActionName = "TopUp";
        
        public const string WithdrawalActionName = "Withdrawal";
        
        public const string TopUpPaymentItemType = "EasyAbpPaymentServicePrepaymentTopUp";
        
        public const string PaymentAccountIdPropertyName = "AccountId";
    }
}
