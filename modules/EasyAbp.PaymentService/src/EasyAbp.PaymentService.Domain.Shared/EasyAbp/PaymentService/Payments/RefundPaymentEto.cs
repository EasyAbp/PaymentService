using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class RefundPaymentEto : IHasExtraProperties
    {
        public Guid? TenantId { get; set; }
        
        public Guid PaymentId { get; set; }
        
        public string DisplayReason { get; set; }
        
        public List<RefundPaymentItemEto> Items { get; set; }

        public Dictionary<string, object> ExtraProperties { get; set; }
    }
}