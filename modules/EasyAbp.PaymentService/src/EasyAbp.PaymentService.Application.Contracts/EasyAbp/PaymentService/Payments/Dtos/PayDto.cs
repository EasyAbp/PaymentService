using System;
using System.Collections.Generic;

namespace EasyAbp.PaymentService.Payments.Dtos
{
    public class PayDto
    {
        public Guid PaymentId { get; set; }
        
        public Dictionary<string, object> ExtraProperties { get; set; }
    }
}