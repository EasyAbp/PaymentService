using System;
using System.Collections.Generic;
using EasyAbp.PaymentService.Refunds;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class PaymentRefundRollbackEto
    {
        public PaymentEto Payment { get; set; }
        
        public RefundEto Refund { get; set; }
    }
}