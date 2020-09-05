using System;
using Volo.Abp.Data;

namespace EasyAbp.PaymentService.Refunds
{
    public interface IRefundItem : IHasExtraProperties
    {
        Guid PaymentItemId { get; }
        
        decimal RefundAmount { get; }

        string CustomerRemark { get; }
        
        string StaffRemark { get; }
    }
}