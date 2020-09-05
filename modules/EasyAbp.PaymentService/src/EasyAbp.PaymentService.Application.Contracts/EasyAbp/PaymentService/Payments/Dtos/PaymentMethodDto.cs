using System;

namespace EasyAbp.PaymentService.Payments.Dtos
{
    [Serializable]
    public class PaymentMethodDto
    {
        public string PaymentMethod { get; set; }
        
        public string Name { get; set; }
    }
}