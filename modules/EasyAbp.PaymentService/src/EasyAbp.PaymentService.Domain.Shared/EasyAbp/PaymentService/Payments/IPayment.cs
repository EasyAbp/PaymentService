using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPayment : IHasExtraProperties
    {
        Guid UserId { get; }

        string PaymentMethod { get; }

        string PayeeAccount { get; }

        string ExternalTradingCode { get; }

        string Currency { get; }

        decimal OriginalPaymentAmount { get; }

        decimal PaymentDiscount { get; }

        decimal ActualPaymentAmount { get; }

        decimal RefundAmount { get; }

        decimal PendingRefundAmount { get; }

        DateTime? CompletionTime { get; }

        DateTime? CanceledTime { get; }

        IEnumerable<IPaymentItem> PaymentItems { get; }
    }
}