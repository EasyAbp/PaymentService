using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class CreatePaymentEto : IHasExtraProperties, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        
        public Guid UserId { get; set; }
        
        public string PaymentMethod { get; set; }

        public string Currency { get; set; }
        
        public ExtraPropertyDictionary ExtraProperties { get; set; }

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