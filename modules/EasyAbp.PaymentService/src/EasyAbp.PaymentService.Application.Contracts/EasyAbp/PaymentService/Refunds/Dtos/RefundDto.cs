using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Refunds.Dtos
{
    [Serializable]
    public class RefundDto : ExtensibleFullAuditedEntityDto<Guid>, IRefund
    {
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

        IEnumerable<IRefundItem> IRefund.RefundItems => RefundItems;
        public List<RefundItemDto> RefundItems { get; set; } = new();
    }
}