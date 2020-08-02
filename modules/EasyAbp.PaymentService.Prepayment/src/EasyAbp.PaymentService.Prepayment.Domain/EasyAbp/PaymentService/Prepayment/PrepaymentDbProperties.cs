namespace EasyAbp.PaymentService.Prepayment
{
    public static class PrepaymentDbProperties
    {
        public static string DbTablePrefix { get; set; } = "EasyAbpPaymentServicePrepayment";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "EasyAbpPaymentServicePrepayment";
    }
}
