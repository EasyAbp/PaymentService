using System;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class PaymentCanceledEto : IMultiTenant
    {
        public Guid? TenantId { get; set; }
        
        public PaymentEto Payment { get; set; }

        public PaymentCanceledEto(PaymentEto payment)
        {
            TenantId = payment.TenantId;
            Payment = payment;
        }
    }
}