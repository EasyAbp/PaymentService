using System;
using JetBrains.Annotations;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentItem
    {
        string ItemType { get; }
        
        string ItemKey { get; }
        
        decimal OriginalPaymentAmount { get; }

        decimal PaymentDiscount { get; }
        
        decimal ActualPaymentAmount { get; }
        
        decimal RefundAmount { get; }
        
        decimal PendingRefundAmount { get; }
    }
}