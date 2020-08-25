using System;
using JetBrains.Annotations;

namespace EasyAbp.PaymentService.Refunds
{
    [Serializable]
    public class CreateRefundItemInput
    {
        public Guid PaymentItemId { get; set; }
        
        public decimal RefundAmount { get; set; }
        
        [CanBeNull]
        public string CustomerRemark { get; set; }
        
        [CanBeNull]
        public string StaffRemark { get; set; }
    }
}