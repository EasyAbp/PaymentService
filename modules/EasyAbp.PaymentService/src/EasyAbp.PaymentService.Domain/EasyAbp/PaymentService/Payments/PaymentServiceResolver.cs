using System;
using System.Collections.Generic;
using System.Linq;
using EasyAbp.PaymentService.Options;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentServiceResolver : IPaymentServiceResolver, ISingletonDependency
    {
        protected readonly Dictionary<string, Type> Providers = new();
        
        public PaymentServiceResolver(
            IOptions<PaymentServiceOptions> options)
        {
            foreach (var provider in options.Value.Providers.GetConfigurations())
            {
                Providers.Add(provider.ProviderName, provider.ProviderType);
            }
        }

        public virtual List<string> GetPaymentMethods()
        {
            return Providers.Keys.ToList();
        }

        public virtual Type GetProviderTypeOrDefault(string paymentMethod)
        {
            return Providers.GetOrDefault(paymentMethod);
        }
    }
}