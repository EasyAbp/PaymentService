using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace EasyAbp.PaymentService.Refunds
{
    [Serializable]
    public class RefundItemEto : IRefundItem, IHasExtraProperties
    {
        public Guid Id { get; set; }

        public Guid PaymentItemId { get; set; }
        
        public decimal RefundAmount { get; set; }
        
        public string CustomerRemark { get; set; }
        
        public string StaffRemark { get; set; }
        
        public Dictionary<string, object> ExtraProperties { get; set; }
    }
}