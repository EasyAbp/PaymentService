using System;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class CreatePaymentItemEto
    {
        public string ItemType { get; set; }
        
        public Guid ItemKey { get; set; }

        public string Currency { get; set; }

        public decimal OriginalPaymentAmount { get; set; }
    }
}