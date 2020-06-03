using System;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class PaymentItemEto : IPaymentItem
    {
        public Guid Id { get; set; }
        
        public string ItemType { get; set; }

        public Guid ItemKey { get; set; }

        public string Currency { get; set; }

        public decimal OriginalPaymentAmount { get; set; }

        public decimal PaymentDiscount { get; set; }

        public decimal ActualPaymentAmount { get; set; }

        public decimal RefundAmount { get; set; }
    }
}