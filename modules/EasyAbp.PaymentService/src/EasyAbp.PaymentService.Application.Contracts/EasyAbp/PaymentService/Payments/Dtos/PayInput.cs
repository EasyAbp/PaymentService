using System;
using System.Collections.Generic;

namespace EasyAbp.PaymentService.Payments.Dtos
{
    public class PayInput
    {
        public Guid PaymentId { get; set; }
        
        public Dictionary<string, object> ExtraProperties { get; set; }
    }
}