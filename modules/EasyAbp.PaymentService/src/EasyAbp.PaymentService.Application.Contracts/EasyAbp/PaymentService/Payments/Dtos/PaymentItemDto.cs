using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Payments.Dtos
{
    public class PaymentItemDto : FullAuditedEntityDto<Guid>
    {
        public string ItemType { get; set; }

        public string ItemKey { get; set; }

        public string Currency { get; set; }

        public decimal OriginalPaymentAmount { get; set; }

        public decimal PaymentDiscount { get; set; }

        public decimal ActualPaymentAmount { get; set; }

        public decimal RefundAmount { get; set; }
        
        public decimal PendingRefundAmount { get; set; }
    }
}