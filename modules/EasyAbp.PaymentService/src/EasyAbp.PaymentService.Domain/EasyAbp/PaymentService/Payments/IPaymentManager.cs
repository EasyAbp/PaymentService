using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.Domain.Services;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentManager : IDomainService
    {
        Task StartPaymentAsync(Payment payment, Dictionary<string, object> configurations = null);

        Task CompletePaymentAsync(Payment payment);
        
        Task StartCancelAsync(Payment payment);

        Task CompleteCancelAsync(Payment payment);

        Task StartRefundAsync(Payment payment, CreateRefundInput input);
        
        Task CompleteRefundAsync(Payment payment, Refund refund);
        
        Task RollbackRefundAsync(Payment payment, Refund refund);
    }
}