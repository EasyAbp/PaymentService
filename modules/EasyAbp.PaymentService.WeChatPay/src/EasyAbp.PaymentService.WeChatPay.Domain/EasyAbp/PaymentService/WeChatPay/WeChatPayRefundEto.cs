using System;
using System.Collections.Generic;
using EasyAbp.PaymentService.Refunds;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class WeChatPayRefundEto
    {
        public Guid? TenantId { get; set; }
        
        public Guid PaymentId { get; set; }
        
        public IEnumerable<RefundInfoModel> RefundInfos { get; set; }
        
        public string DisplayReason { get; set; }
    }
}