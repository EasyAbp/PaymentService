using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class PaymentEto : IPayment, IMultiTenant, IHasExtraProperties
    {
        public Guid UserId { get; set; }

        public Guid Id { get; set; }
        
        public Guid? TenantId { get; set; }
        
        public string PaymentMethod { get; set; }
        
        public string PayeeAccount { get; set; }

        public string ExternalTradingCode { get; set; }
        
        public string Currency { get; set; }
        
        public decimal OriginalPaymentAmount { get; set; }

        public decimal PaymentDiscount { get; set; }
        
        public decimal ActualPaymentAmount { get; set; }
        
        public decimal RefundAmount { get; set; }
        
        public decimal PendingRefundAmount { get; set; }
        
        public DateTime? CompletionTime { get; set; }
        
        public DateTime? CanceledTime { get; set; }
        
        public DateTime CreationTime { get; set; }
    
        public Dictionary<string, object> ExtraProperties { get; set; }

        public List<PaymentItemEto> PaymentItems { get; set; }
    }
}