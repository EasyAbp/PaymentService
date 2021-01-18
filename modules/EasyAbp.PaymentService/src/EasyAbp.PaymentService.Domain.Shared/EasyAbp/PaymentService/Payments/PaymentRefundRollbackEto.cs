using System;
using System.Collections.Generic;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class PaymentRefundRollbackEto : IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public PaymentEto Payment { get; set; }
        
        public RefundEto Refund { get; set; }

        public PaymentRefundRollbackEto(
            PaymentEto payment,
            RefundEto refund)
        {
            TenantId = payment.TenantId;
            Payment = payment;
            Refund = refund;
        }
    }
}