using System;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class PaymentRefundCompletedEto : IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public PaymentEto Payment { get; set; }
        
        public RefundEto Refund { get; set; }

        public PaymentRefundCompletedEto(
            PaymentEto payment,
            RefundEto refund)
        {
            TenantId = payment.TenantId;
            Payment = payment;
            Refund = refund;
        }
    }
}