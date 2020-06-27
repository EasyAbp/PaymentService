using System;

namespace EasyAbp.PaymentService.Refunds
{
    public interface IRefund
    {
        Guid PaymentId { get; }
        
        Guid PaymentItemId { get; }
        
        string RefundPaymentMethod { get; }
        
        string ExternalTradingCode { get; }
        
        string Currency { get; }
        
        decimal RefundAmount { get; }

        string CustomerRemark { get; }
        
        string StaffRemark { get; }
        
        public DateTime? CompletedTime { get; }
        
        public DateTime? CanceledTime { get; }
    }
}