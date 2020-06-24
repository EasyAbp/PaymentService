using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace EasyAbp.PaymentService.Refunds
{
    [Serializable]
    public class RefundEto : IRefund, IHasExtraProperties
    {
        public Guid PaymentId { get; set; }
        
        public Guid PaymentItemId { get; set; }
        
        public string RefundPaymentMethod { get; set; }
        
        public string ExternalTradingCode { get; set; }
        
        public string Currency { get; set; }
        
        public decimal RefundAmount { get; set; }
        
        public string CustomerRemark { get; set; }
        
        public string StaffRemark { get; set; }
        
        public DateTime? CompletedTime { get; set; }
        
        public DateTime? CancelledTime { get; set; }

        public Dictionary<string, object> ExtraProperties { get; set; }
    }
}