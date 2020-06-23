using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentServiceProvider
    {
        Task<Payment> PayAsync(Payment payment, Dictionary<string, object> configurations);
        
        Task<Payment> CancelAsync(Payment payment);

        Task<Payment> RefundAsync(Payment payment, IEnumerable<RefundInfoModel> refundInfos, string displayReason = null);
    }
}