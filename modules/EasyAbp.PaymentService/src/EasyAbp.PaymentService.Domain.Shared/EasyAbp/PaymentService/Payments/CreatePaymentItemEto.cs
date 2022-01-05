using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.PaymentService.Payments
{
    public class CreatePaymentItemEto : ExtensibleObject
    {
        public string ItemType { get; set; }
        
        public string ItemKey { get; set; }

        public decimal OriginalPaymentAmount { get; set; }
    }
}