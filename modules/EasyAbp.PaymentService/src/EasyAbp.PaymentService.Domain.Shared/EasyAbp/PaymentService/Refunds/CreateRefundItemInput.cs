using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Data;

namespace EasyAbp.PaymentService.Refunds
{
    [Serializable]
    public class CreateRefundItemInput : IHasExtraProperties
    {
        public Guid PaymentItemId { get; set; }
        
        public decimal RefundAmount { get; set; }
        
        [CanBeNull]
        public string CustomerRemark { get; set; }
        
        [CanBeNull]
        public string StaffRemark { get; set; }

        public Dictionary<string, object> ExtraProperties { get; set; } = new Dictionary<string, object>();
    }
}