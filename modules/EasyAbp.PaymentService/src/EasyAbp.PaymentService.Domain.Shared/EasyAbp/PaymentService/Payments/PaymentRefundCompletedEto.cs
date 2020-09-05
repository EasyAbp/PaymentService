using System;
using EasyAbp.PaymentService.Refunds;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class PaymentRefundCompletedEto
    {
        public PaymentEto Payment { get; set; }
        
        public RefundEto Refund { get; set; }
    }
}