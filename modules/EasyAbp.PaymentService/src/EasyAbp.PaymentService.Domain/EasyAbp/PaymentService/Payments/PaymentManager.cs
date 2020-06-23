using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.Domain.Services;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentManager : DomainService, IPaymentManager
    {
        private readonly IPaymentServiceResolver _paymentServiceResolver;

        public PaymentManager(IPaymentServiceResolver paymentServiceResolver)
        {
            _paymentServiceResolver = paymentServiceResolver;
        }
        
        public virtual async Task<Payment> PayAsync(Payment payment, Dictionary<string, object> configurations = null)
        {
            var provider = GetProvider(payment);

            // Todo: payment discount

            await provider.PayAsync(payment, configurations);

            return payment;
        }

        public virtual async Task<Payment> CancelAsync(Payment payment)
        {
            var provider = GetProvider(payment);

            await provider.CancelAsync(payment);

            return payment;
        }

        public virtual async Task<Payment> RefundAsync(Payment payment, IEnumerable<RefundInfoModel> refundInfos, string displayReason = null)
        {
            var provider = GetProvider(payment);

            await provider.RefundAsync(payment, refundInfos, displayReason);

            return payment;
        }
        
        protected virtual IPaymentServiceProvider GetProvider(IPayment payment)
        {
            var providerType = _paymentServiceResolver.GetProviderTypeOrDefault(payment.PaymentMethod) ??
                               throw new UnknownPaymentMethodException(payment.PaymentMethod);

            return ServiceProvider.GetService(providerType) as IPaymentServiceProvider ??
                   throw new UnknownPaymentMethodException(payment.PaymentMethod);
        }
    }
}