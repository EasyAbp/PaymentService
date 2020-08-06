using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.PaymentService.Payments
{
    public abstract class PaymentServiceProvider : IPaymentServiceProvider, ITransientDependency
    {
        public virtual Task OnPaymentStartedAsync(Payment payment, Dictionary<string, object> configurations)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnCancelStartedAsync(Payment payment)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnRefundStartedAsync(Payment payment, IEnumerable<Refund> refunds, string displayReason = null)
        {
            return Task.CompletedTask;
        }
    }
}