using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class CreatePaymentEto : IMultiTenant, IHasExtraProperties
    {
        public Guid? TenantId { get; set; }
        
        public Guid UserId { get; set; }
        
        public string PaymentMethod { get; set; }

        public string Currency { get; set; }
        
        public Dictionary<string, object> ExtraProperties { get; set; } = new Dictionary<string, object>();

        public List<CreatePaymentItemEto> PaymentItems { get; set; }
    }
}