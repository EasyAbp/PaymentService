using System;

namespace EasyAbp.PaymentService.Refunds
{
    public class CreateRefundEto
    {
        public Guid? TenantId { get; set; }
        
        public Guid PaymentId { get; set; }

        public Guid PaymentItemId { get; set; }

        public string RefundPaymentMethod { get; set; }

        public string ExternalTradingCode { get; set; }

        public string Currency { get; set; }

        public decimal RefundAmount { get; set; }

        public string CustomerRemark { get; set; }

        public string StaffRemark { get; set; }
    }
}