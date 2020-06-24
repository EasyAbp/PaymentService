using System;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class PaymentCancelCompletedEto
    {
        public PaymentEto Payment { get; set; }
    }
}