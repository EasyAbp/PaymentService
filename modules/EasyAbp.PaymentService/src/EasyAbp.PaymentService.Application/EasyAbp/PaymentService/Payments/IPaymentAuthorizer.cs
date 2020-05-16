using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentAuthorizer
    {
        Task<bool> IsPaymentItemAllowedAsync(Payment payment, PaymentItem paymentItem,
            Dictionary<string, object> inputExtraProperties);
    }
}