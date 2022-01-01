using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.PaymentService.Options.PaymentServiceProviders
{
    public class PaymentServiceProviderConfigurationProvider : IPaymentServiceProviderConfigurationProvider, ITransientDependency
    {
        private readonly PaymentServiceOptions _options;

        public PaymentServiceProviderConfigurationProvider(IOptions<PaymentServiceOptions> options)
        {
            _options = options.Value;
        }
        
        public PaymentServiceProviderConfiguration Get(string providerName)
        {
            return _options.Providers.GetConfiguration(providerName);
        }
    }
}