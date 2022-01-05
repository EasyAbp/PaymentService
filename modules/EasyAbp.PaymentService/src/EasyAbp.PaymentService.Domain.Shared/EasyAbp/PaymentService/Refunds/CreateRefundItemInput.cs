using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.PaymentService.Refunds
{
    public class CreateRefundItemInput : ExtensibleObject
    {
        public Guid PaymentItemId { get; set; }
        
        public decimal RefundAmount { get; set; }
        
        [CanBeNull]
        public string CustomerRemark { get; set; }
        
        [CanBeNull]
        public string StaffRemark { get; set; }
    }
}