using System;
using System.Collections.Generic;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class WeChatPayRefundJobArgs : IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid PaymentId { get; set; }

        public Guid? RefundId { get; set; }

        public string OutRefundNo { get; set; }

        public decimal RefundAmount { get; set; }

        public string DisplayReason { get; set; }

        /// <summary>
        /// Use a <see cref="Refund"/> entity to handle the refund request.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="paymentId"></param>
        /// <param name="refundId"></param>
        public WeChatPayRefundJobArgs(
            Guid? tenantId,
            Guid paymentId,
            Guid refundId)
        {
            TenantId = tenantId;
            PaymentId = paymentId;
            RefundId = refundId;
        }

        /// <summary>
        /// Handle the refund request without a <see cref="Refund"/> entity. 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="paymentId"></param>
        /// <param name="outRefundNo"></param>
        /// <param name="refundAmount"></param>
        /// <param name="displayReason"></param>
        public WeChatPayRefundJobArgs(
            Guid? tenantId,
            Guid paymentId,
            [NotNull] string outRefundNo,
            decimal refundAmount,
            [CanBeNull] string displayReason)
        {
            TenantId = tenantId;
            PaymentId = paymentId;
            OutRefundNo = Check.NotNullOrWhiteSpace(outRefundNo, nameof(outRefundNo));
            RefundAmount = refundAmount;
            DisplayReason = displayReason;
        }
    }
}