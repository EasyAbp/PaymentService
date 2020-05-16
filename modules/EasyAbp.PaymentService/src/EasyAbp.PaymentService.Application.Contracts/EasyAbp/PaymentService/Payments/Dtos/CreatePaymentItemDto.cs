using System;
using System.ComponentModel;

namespace EasyAbp.PaymentService.Payments.Dtos
{
    public class CreatePaymentItemDto
    {
        [DisplayName("PaymentItemItemType")]
        public string ItemType { get; set; }
        
        [DisplayName("PaymentItemItemKey")]
        public Guid ItemKey { get; set; }

        [DisplayName("PaymentItemCurrency")]
        public string Currency { get; set; }

        [DisplayName("PaymentItemOriginalPaymentAmount")]
        public decimal OriginalPaymentAmount { get; set; }
    }
}