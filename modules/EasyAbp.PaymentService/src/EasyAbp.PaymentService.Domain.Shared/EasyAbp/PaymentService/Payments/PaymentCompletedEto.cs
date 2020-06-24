using System;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class PaymentCompletedEto
    {
        public PaymentEto Payment { get; set; }
    }
}