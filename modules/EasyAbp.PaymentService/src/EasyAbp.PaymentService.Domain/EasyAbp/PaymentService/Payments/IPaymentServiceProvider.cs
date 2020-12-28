using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.Data;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentServiceProvider
    {
        Task OnPaymentStartedAsync(Payment payment, ExtraPropertyDictionary configurations);
        
        Task OnCancelStartedAsync(Payment payment);

        Task OnRefundStartedAsync(Payment payment, Refund refund);
    }
}