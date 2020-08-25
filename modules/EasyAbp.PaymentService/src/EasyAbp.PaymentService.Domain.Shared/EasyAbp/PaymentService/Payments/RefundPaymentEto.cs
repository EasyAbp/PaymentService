using System;
using System.Collections.Generic;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Payments
{
    [Serializable]
    public class RefundPaymentEto : IMultiTenant
    {
        public Guid? TenantId { get; set; }
        
        public CreateRefundInput CreateRefundInput { get; set; }
    }
}