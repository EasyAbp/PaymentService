using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class CreatePaymentEto : IHasExtraProperties
    {
        public Guid? TenantId { get; set; }
        
        public Guid UserId { get; set; }
        
        public string PaymentMethod { get; set; }

        public string Currency { get; set; }
        
        public Dictionary<string, object> ExtraProperties { get; set; }

        public List<CreatePaymentItemEto> PaymentItems { get; set; }
    }
}