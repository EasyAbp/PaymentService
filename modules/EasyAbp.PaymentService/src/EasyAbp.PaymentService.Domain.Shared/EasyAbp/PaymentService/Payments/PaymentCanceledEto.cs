using System;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class PaymentCanceledEto
    {
        public PaymentEto Payment { get; set; }
    }
}