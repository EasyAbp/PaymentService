using EasyAbp.PaymentService.Payments;

namespace EasyAbp.PaymentService.Refunds
{
    public class RefundInfoModel
    {
        public PaymentItem PaymentItem { get; set; }
        
        public decimal RefundAmount { get; set; }
        
        public string CustomerRemark { get; set; }
        
        public string StaffRemark { get; set; }
    }
}