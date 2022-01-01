using System;
using System.Collections.Generic;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class CloseWeChatPayOrderEto : IMultiTenant
    {
        public Guid? TenantId { get; set; }
        
        public Guid PaymentId { get; set; }
        
        public string OutTradeNo { get; set; }
        
        public string AppId { get; set; }
        
        public string MchId { get; set; }

        public CloseWeChatPayOrderEto(Guid? tenantId, Guid paymentId, string outTradeNo, string appId, string mchId)
        {
            TenantId = tenantId;
            PaymentId = paymentId;
            OutTradeNo = outTradeNo;
            AppId = appId;
            MchId = mchId;
        }
    }
}