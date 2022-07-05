using System;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.WeChatPay.Background;

[Serializable]
public class EmptyJobArgs : IMultiTenant
{
    public Guid? TenantId { get; set; }

    public EmptyJobArgs(Guid? tenantId)
    {
        TenantId = tenantId;
    }
}