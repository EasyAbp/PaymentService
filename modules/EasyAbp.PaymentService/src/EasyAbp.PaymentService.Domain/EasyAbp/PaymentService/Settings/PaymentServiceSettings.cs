namespace EasyAbp.PaymentService.Settings
{
    public static class PaymentServiceSettings
    {
        public const string GroupName = "EasyAbp.PaymentService";

        /* Add constants for setting names. Example:
         * public const string MySettingName = GroupName + ".MySettingName";
         */
        
        public static class FreePaymentMethod
        {
            private const string PaymentMethodName = GroupName + ".Free";
            
            public const string DefaultPayeeAccount = PaymentMethodName + ".DefaultPayeeAccount";

        }
    }
}