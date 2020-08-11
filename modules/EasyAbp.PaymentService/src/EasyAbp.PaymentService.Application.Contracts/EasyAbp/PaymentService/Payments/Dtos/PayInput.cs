using System;
using System.Collections.Generic;

namespace EasyAbp.PaymentService.Payments.Dtos
{
    public class PayInput
    {
        public Dictionary<string, object> ExtraProperties { get; set; } = new Dictionary<string, object>();
    }
}