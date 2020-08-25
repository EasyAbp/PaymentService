using System;

namespace EasyAbp.PaymentService.Refunds
{
    public interface IRefundItem
    {
        Guid PaymentItemId { get; }
        
        decimal RefundAmount { get; }

        string CustomerRemark { get; }
        
        string StaffRemark { get; }
    }
}