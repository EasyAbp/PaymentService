using System;
using JetBrains.Annotations;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentItem
    {
        string ItemType { get; }
        
        Guid ItemKey { get; }
        
        string Currency { get; }
        
        decimal OriginalPaymentAmount { get; }

        decimal PaymentDiscount { get; }
        
        decimal ActualPaymentAmount { get; }
        
        decimal RefundAmount { get; }
        
        decimal PendingRefundAmount { get; }
    }
}