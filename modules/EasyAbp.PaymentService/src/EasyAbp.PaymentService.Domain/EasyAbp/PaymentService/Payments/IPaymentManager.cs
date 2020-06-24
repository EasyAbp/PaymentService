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

        Task StartRefundAsync(Payment payment, IEnumerable<RefundInfoModel> refundInfos, string displayReason = null);
        
        Task CompleteRefundAsync(Payment payment, IEnumerable<Refund> refunds);
        
        Task RollbackRefundAsync(Payment payment, IEnumerable<Refund> refunds);
    }
}