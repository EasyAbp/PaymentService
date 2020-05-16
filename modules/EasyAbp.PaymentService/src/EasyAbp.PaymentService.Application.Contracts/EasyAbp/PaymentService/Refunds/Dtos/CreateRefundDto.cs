using System;
using System.ComponentModel;
namespace EasyAbp.PaymentService.Refunds.Dtos
{
    public class CreateRefundDto
    {
        [DisplayName("PaymentId")]
        public Guid PaymentId { get; set; }

        [DisplayName("RefundPaymentItemId")]
        public Guid PaymentItemId { get; set; }

        [DisplayName("RefundRefundPaymentMethod")]
        public string RefundPaymentMethod { get; set; }

        [DisplayName("RefundExternalTradingCode")]
        public string ExternalTradingCode { get; set; }

        [DisplayName("RefundCurrency")]
        public string Currency { get; set; }

        [DisplayName("RefundRefundAmount")]
        public decimal RefundAmount { get; set; }

        [DisplayName("RefundCustomerRemark")]
        public string CustomerRemark { get; set; }

        [DisplayName("RefundStaffRemark")]
        public string StaffRemark { get; set; }
    }
}