using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentServiceProvider
    {
        Task OnStartPaymentAsync(Payment payment, Dictionary<string, object> configurations);
        
        Task OnStartCancelAsync(Payment payment);

        Task OnStartRefundAsync(Payment payment, IEnumerable<Refund> refunds, string displayReason = null);
    }
}