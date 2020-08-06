namespace EasyAbp.PaymentService
{
    public static class PaymentServiceDbProperties
    {
        public static string DbTablePrefix { get; set; } = "EasyAbpPaymentService";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "EasyAbpPaymentService";
    }
}
