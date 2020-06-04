using System;
using System.Collections.Generic;

namespace EasyAbp.PaymentService.Payments
{
    public class CreatePaymentEto
    {
        public Guid? TenantId { get; set; }
        
        public Guid UserId { get; set; }
        
        public string PaymentMethod { get; set; }

        public string Currency { get; set; }
        
        public Dictionary<string, object> ExtraProperties { get; set; }

        public List<CreatePaymentItemEto> PaymentItems { get; set; }
    }
}