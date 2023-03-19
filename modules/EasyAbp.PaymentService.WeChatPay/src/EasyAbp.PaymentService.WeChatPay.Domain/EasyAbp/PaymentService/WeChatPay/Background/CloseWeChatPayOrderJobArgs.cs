using System;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.WeChatPay.Background;

[Serializable]
public class CloseWeChatPayOrderJobArgs : IMultiTenant
{
    public Guid? TenantId { get; set; }

    public Guid PaymentId { get; set; }

    public string OutTradeNo { get; set; }

    public string AppId { get; set; }

    public string MchId { get; set; }

    public CloseWeChatPayOrderJobArgs()
    {
    }

    public CloseWeChatPayOrderJobArgs(Guid? tenantId, Guid paymentId, string outTradeNo, string appId, string mchId)
    {
        TenantId = tenantId;
        PaymentId = paymentId;
        OutTradeNo = outTradeNo;
        AppId = appId;
        MchId = mchId;
    }
}