using System;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentItemEto : ExtensibleObject, IPaymentItem
    {
        public Guid Id { get; set; }
        
        public string ItemType { get; set; }

        public string ItemKey { get; set; }

        public decimal OriginalPaymentAmount { get; set; }

        public decimal PaymentDiscount { get; set; }

        public decimal ActualPaymentAmount { get; set; }

        public decimal RefundAmount { get; set; }
        
        public decimal PendingRefundAmount { get; set; }
    }
}