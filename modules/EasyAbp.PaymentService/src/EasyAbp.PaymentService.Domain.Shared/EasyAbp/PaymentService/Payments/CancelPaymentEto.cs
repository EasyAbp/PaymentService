using System;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class CancelPaymentEto
    {
        public Guid? TenantId { get; set; }
        
        public Guid PaymentId { get; set; }
    }
}