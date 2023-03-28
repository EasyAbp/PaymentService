using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.PaymentService.Refunds
{
    public class RefundEto : ExtensibleObject, IRefund, IMultiTenant
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

        IEnumerable<IRefundItem> IRefund.RefundItems => RefundItems;
        public List<RefundItemEto> RefundItems { get; set; } = new();
    }
}