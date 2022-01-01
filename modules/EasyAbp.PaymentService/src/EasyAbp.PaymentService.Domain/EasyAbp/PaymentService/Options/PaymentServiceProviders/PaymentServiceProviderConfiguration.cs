using System;

namespace EasyAbp.PaymentService.Options.PaymentServiceProviders
{
    public class PaymentServiceProviderConfiguration
    {
        public Type ProviderType { get; set; }
        
        public string ProviderName { get; set; }

        public PaymentServiceProviderConfiguration()
        {
            
        }
        
        public PaymentServiceProviderConfiguration(Type providerType, string providerName)
        {
            ProviderType = providerType;
            ProviderName = providerName;
        }
    }
}