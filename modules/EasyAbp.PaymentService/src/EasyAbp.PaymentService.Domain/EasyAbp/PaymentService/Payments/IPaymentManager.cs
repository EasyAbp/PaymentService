using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.Domain.Services;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentManager : IDomainService
    {
        Task<Payment> PayAsync(Payment payment, Dictionary<string, object> configurations = null);
        
        Task<Payment> CancelAsync(Payment payment);

        Task<Payment> RefundAsync(Payment payment, IEnumerable<RefundInfoModel> refundInfos, string displayReason = null);
    }
}