using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace EasyAbp.PaymentService.Refunds
{
    public interface IRefund : IHasExtraProperties
    {
        Guid Id { get; }

        Guid PaymentId { get; }

        string RefundPaymentMethod { get; }

        string ExternalTradingCode { get; }

        string Currency { get; }

        decimal RefundAmount { get; }

        string DisplayReason { get; }

        string CustomerRemark { get; }

        string StaffRemark { get; }

        public DateTime? CompletedTime { get; }

        public DateTime? CanceledTime { get; }

        IEnumerable<IRefundItem> RefundItems { get; }
    }
}