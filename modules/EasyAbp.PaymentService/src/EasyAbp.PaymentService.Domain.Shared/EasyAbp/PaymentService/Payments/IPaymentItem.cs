using System;
using JetBrains.Annotations;
using Volo.Abp.Data;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentItem : IHasExtraProperties
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