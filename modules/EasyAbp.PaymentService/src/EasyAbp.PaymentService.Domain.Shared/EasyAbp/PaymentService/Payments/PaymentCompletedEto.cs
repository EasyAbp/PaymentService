using System;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class PaymentCompletedEto : IMultiTenant
    {
        public Guid? TenantId { get; set; }
        
        public PaymentEto Payment { get; set; }

        public PaymentCompletedEto(PaymentEto payment)
        {
            TenantId = payment.TenantId;
            Payment = payment;
        }
    }
}