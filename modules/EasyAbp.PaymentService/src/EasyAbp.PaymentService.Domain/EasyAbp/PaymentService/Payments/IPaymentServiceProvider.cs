using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentServiceProvider
    {
        Task<Payment> PayAsync(Payment payment, Dictionary<string, object> configurations);
    }
}