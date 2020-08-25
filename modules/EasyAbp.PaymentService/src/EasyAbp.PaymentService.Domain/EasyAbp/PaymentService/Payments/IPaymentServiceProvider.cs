using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentServiceProvider
    {
        Task OnPaymentStartedAsync(Payment payment, Dictionary<string, object> configurations);
        
        Task OnCancelStartedAsync(Payment payment);

        Task OnRefundStartedAsync(Payment payment, Refund refund);
    }
}