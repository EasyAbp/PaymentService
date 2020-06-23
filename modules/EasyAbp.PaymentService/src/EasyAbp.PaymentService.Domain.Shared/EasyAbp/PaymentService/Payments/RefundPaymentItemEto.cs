using System;

namespace EasyAbp.PaymentService.Payments
{
    public class RefundPaymentItemEto
    {
        public Guid PaymentItemId { get; set; }
        
        public decimal RefundAmount { get; set; }
        
        public string CustomerRemark { get; set; }
        
        public string StaffRemark { get; set; }
    }
}