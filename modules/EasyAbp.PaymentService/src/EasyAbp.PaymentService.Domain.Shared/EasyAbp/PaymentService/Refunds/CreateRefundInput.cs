using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Refunds
{
    [Serializable]
    public class CreateRefundInput
    {
        public Guid PaymentId { get; set; }
        
        [CanBeNull]
        public string DisplayReason { get; set; }

        [CanBeNull]
        public string CustomerRemark { get; set; }
        
        [CanBeNull]
        public string StaffRemark { get; set; }
        
        public List<CreateRefundItemInput> RefundItems { get; set; } = new List<CreateRefundItemInput>();
    }
}