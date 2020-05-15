namespace EasyAbp.PaymentService.WeChatPay.Settings
{
    public static class WeChatPaySettings
    {
        public const string GroupName = "EasyAbp.PaymentService.WeChatPay";

        /* Add constants for setting names. Example:
         * public const string MySettingName = GroupName + ".MySettingName";
         */
        
        public const string MchId = GroupName + ".MchId";
        public const string ApiKey = GroupName + ".ApiKey";
        public const string IsSandBox = GroupName + ".IsSandBox";
        public const string NotifyUrl = GroupName + ".NotifyUrl";
        public const string RefundNotifyUrl = GroupName + ".RefundNotifyUrl";
        public const string CertificatePath = GroupName + ".CertificatePath";
        public const string CertificateSecret = GroupName + ".CertificateSecret";
    }
}