using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Payments.Dtos
{
    [Serializable]
    public class PaymentItemDto : ExtensibleFullAuditedEntityDto<Guid>, IPaymentItem
    {
        public string ItemType { get; set; }

        public string ItemKey { get; set; }

        public decimal OriginalPaymentAmount { get; set; }

        public decimal PaymentDiscount { get; set; }

        public decimal ActualPaymentAmount { get; set; }

        public decimal RefundAmount { get; set; }
        
        public decimal PendingRefundAmount { get; set; }
    }
}