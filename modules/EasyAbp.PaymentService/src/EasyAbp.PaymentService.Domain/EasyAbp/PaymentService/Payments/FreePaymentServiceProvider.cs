using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace EasyAbp.PaymentService.Payments
{
    public class FreePaymentServiceProvider : IPaymentServiceProvider, ITransientDependency
    {
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentRepository _paymentRepository;
        public const string PaymentMethod = "Free";
        
        public FreePaymentServiceProvider(
            IPaymentManager paymentManager,
            IPaymentRepository paymentRepository)
        {
            _paymentManager = paymentManager;
            _paymentRepository = paymentRepository;
        }

        public async Task OnStartPaymentAsync(Payment payment, Dictionary<string, object> configurations)
        {
            if (payment.ActualPaymentAmount != decimal.Zero)
            {
                throw new PaymentAmountInvalidException(payment.ActualPaymentAmount, PaymentMethod);
            }
            
            payment.SetPayeeAccount("None");
            
            payment.SetExternalTradingCode(payment.Id.ToString());

            await _paymentManager.CompletePaymentAsync(payment);

            await _paymentRepository.UpdateAsync(payment, true);
        }

        public virtual async Task OnStartCancelAsync(Payment payment)
        {
            await _paymentManager.CompleteCancelAsync(payment);
        }

        public virtual Task OnStartRefundAsync(Payment payment, IEnumerable<Refund> refunds, string displayReason = null)
        {
            throw new NotSupportedException();
        }
    }
}