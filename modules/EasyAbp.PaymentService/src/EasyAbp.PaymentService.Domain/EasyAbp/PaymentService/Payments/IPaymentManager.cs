using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentManager : IDomainService
    {
        Task StartPaymentAsync(Payment payment, ExtraPropertyDictionary configurations = null);

        Task CompletePaymentAsync(Payment payment);
        
        Task StartCancelAsync(Payment payment);

        Task CompleteCancelAsync(Payment payment);

        Task StartRefundAsync(Payment payment, CreateRefundInput input);
        
        Task CompleteRefundAsync(Payment payment, Refund refund);
        
        Task RollbackRefundAsync(Payment payment, Refund refund);
    }
}