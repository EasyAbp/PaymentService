using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentEto : ExtensibleObject, IPayment, IMultiTenant
    {
        public Guid UserId { get; set; }

        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        public string PaymentMethod { get; set; }

        public string PayeeAccount { get; set; }

        public string ExternalTradingCode { get; set; }

        public string Currency { get; set; }

        public decimal OriginalPaymentAmount { get; set; }

        public decimal PaymentDiscount { get; set; }

        public decimal ActualPaymentAmount { get; set; }

        public decimal RefundAmount { get; set; }

        public decimal PendingRefundAmount { get; set; }

        public DateTime? CompletionTime { get; set; }

        public DateTime? CanceledTime { get; set; }

        public DateTime CreationTime { get; set; }

        IEnumerable<IPaymentItem> IPayment.PaymentItems => PaymentItems;
        public List<PaymentItemEto> PaymentItems { get; set; }
    }
}