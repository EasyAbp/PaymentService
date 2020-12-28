using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Refunds
{
    [Serializable]
    public class RefundEto : IRefund, IMultiTenant, IHasExtraProperties
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        public Guid PaymentId { get; set; }
        
        public string RefundPaymentMethod { get; set; }
        
        public string ExternalTradingCode { get; set; }
        
        public string Currency { get; set; }
        
        public decimal RefundAmount { get; set; }
        
        public string DisplayReason { get; set; }

        public string CustomerRemark { get; set; }
        
        public string StaffRemark { get; set; }
        
        public DateTime? CompletedTime { get; set; }
        
        public DateTime? CanceledTime { get; set; }

        public ExtraPropertyDictionary ExtraProperties { get; set; }
        
        public List<RefundItemEto> RefundItems { get; set; } = new List<RefundItemEto>();
    }
}