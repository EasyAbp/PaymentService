using System;

namespace EasyAbp.PaymentService.Refunds
{
    [Serializable]
    public class RefundItemEto : IRefundItem
    {
        public Guid Id { get; set; }

        public Guid PaymentItemId { get; set; }
        
        public decimal RefundAmount { get; set; }
        
        public string CustomerRemark { get; set; }
        
        public string StaffRemark { get; set; }
    }
}