using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentPayeeAccountProvider : IPaymentPayeeAccountProvider, ITransientDependency
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISettingProvider _settingProvider;

        public PaymentPayeeAccountProvider(
            IPaymentRepository paymentRepository,
            ISettingProvider settingProvider)
        {
            _paymentRepository = paymentRepository;
            _settingProvider = settingProvider;
        }
        public async Task<string> GetPayeeAccountAsync(Payment payment, Dictionary<string, object> inputExtraProperties)
        {
            // Todo: support multi-store.
            
            var payeeAccount = await _settingProvider.GetOrNullAsync(
                PaymentServiceSettings.GroupName + "." + payment.PaymentMethod + ".DefaultPayeeAccount");

            if (payeeAccount == null)
            {
                throw new PayeeAccountNotFoundException(payment.PaymentMethod);
            }
            
            return payeeAccount;
        }
    }
}