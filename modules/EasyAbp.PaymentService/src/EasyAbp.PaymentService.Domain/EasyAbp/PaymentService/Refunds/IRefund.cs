using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Refunds
{
    public interface IRefund : IAggregateRoot<Guid>, IMultiTenant
    {
        Guid PaymentId { get; }
        
        Guid PaymentItemId { get; }
        
        string RefundPaymentMethod { get; }
        
        string ExternalTradingCode { get; }
        
        string Currency { get; }
        
        decimal RefundAmount { get; }

        string CustomerRemark { get; }
        
        string StaffRemark { get; }
    }
}