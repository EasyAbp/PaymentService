using System.Collections.Generic;
using EasyAbp.PaymentService.Refunds;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentRefundCompletedEto
    {
        public PaymentEto Payment { get; set; }
        
        public RefundEto Refund { get; set; }
    }
}