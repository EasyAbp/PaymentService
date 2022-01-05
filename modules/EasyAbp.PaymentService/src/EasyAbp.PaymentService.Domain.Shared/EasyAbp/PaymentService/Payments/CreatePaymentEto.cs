using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.PaymentService.Payments
{
    public class CreatePaymentEto : ExtensibleObject, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        
        public Guid UserId { get; set; }
        
        public string PaymentMethod { get; set; }

        public string Currency { get; set; }
        
        public List<CreatePaymentItemEto> PaymentItems { get; set; }

        public CreatePaymentEto(
            Guid? tenantId,
            Guid userId,
            string paymentMethod,
            string currency,
            List<CreatePaymentItemEto> paymentItems)
        {
            TenantId = tenantId;
            UserId = userId;
            PaymentMethod = paymentMethod;
            Currency = currency;
            PaymentItems = paymentItems;

            ExtraProperties = new ExtraPropertyDictionary();
        }
    }
}