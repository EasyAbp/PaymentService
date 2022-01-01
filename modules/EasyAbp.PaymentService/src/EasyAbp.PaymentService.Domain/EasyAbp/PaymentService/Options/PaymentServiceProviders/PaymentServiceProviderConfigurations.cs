using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp;

namespace EasyAbp.PaymentService.Options.PaymentServiceProviders
{
    public class PaymentServiceProviderConfigurations
    {
        private readonly Dictionary<string, PaymentServiceProviderConfiguration> _providers;
        
        public PaymentServiceProviderConfigurations()
        {
            _providers = new Dictionary<string, PaymentServiceProviderConfiguration>();
        }

        public PaymentServiceProviderConfigurations Configure<TProvider>(
            string providerName,
            Action<PaymentServiceProviderConfiguration> configureAction = null)
        {
            return Configure(typeof(TProvider), providerName, configureAction);
        }

        public PaymentServiceProviderConfigurations Configure(
            Type provideType,
            [NotNull] string providerName,
            Action<PaymentServiceProviderConfiguration> configureAction = null)
        {
            Check.NotNullOrWhiteSpace(providerName, nameof(providerName));

            configureAction ??= _ => {};
            
            configureAction(
                _providers.GetOrAdd(
                    providerName,
                    () => new PaymentServiceProviderConfiguration(provideType, providerName)
                )
            );

            return this;
        }

        public PaymentServiceProviderConfigurations ConfigureAll(Action<string, PaymentServiceProviderConfiguration> configureAction)
        {
            foreach (var provider in _providers)
            {
                configureAction(provider.Key, provider.Value);
            }
            
            return this;
        }

        [NotNull]
        public PaymentServiceProviderConfiguration GetConfiguration([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return _providers.GetOrDefault(name);
        }
        
        public IEnumerable<PaymentServiceProviderConfiguration> GetConfigurations()
        {
            return _providers.Values;
        }
    }
}