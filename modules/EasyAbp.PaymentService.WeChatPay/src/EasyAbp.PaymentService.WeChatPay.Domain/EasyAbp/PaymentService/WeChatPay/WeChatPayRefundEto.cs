using System;
using System.Collections.Generic;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class WeChatPayRefundEto : IMultiTenant
    {
        public Guid? TenantId { get; set; }
        
        public Guid PaymentId { get; set; }
        
        public Refund Refund { get; set; }

        public WeChatPayRefundEto(
            Guid paymentId,
            Refund refund)
        {
            TenantId = refund.TenantId;
            PaymentId = paymentId;
            Refund = refund;
        }
    }
}