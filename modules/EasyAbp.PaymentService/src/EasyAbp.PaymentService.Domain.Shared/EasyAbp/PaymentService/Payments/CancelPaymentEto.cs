using System;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class CancelPaymentEto : IMultiTenant
    {
        public Guid? TenantId { get; set; }
        
        public Guid PaymentId { get; set; }

        public CancelPaymentEto(Guid? tenantId, Guid paymentId)
        {
            TenantId = tenantId;
            PaymentId = paymentId;
        }
    }
}