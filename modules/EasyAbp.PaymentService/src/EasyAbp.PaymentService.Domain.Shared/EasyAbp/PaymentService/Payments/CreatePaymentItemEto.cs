using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class CreatePaymentItemEto : IHasExtraProperties
    {
        public string ItemType { get; set; }
        
        public Guid ItemKey { get; set; }

        public string Currency { get; set; }

        public decimal OriginalPaymentAmount { get; set; }
        
        public Dictionary<string, object> ExtraProperties { get; set; } = new Dictionary<string, object>();
    }
}