namespace EasyAbp.PaymentService.WeChatPay
{
    public static class WeChatPayDbProperties
    {
        public static string DbTablePrefix { get; set; } = "EasyAbpPaymentServiceWeChatPay";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "EasyAbpPaymentServiceWeChatPay";
    }
}
