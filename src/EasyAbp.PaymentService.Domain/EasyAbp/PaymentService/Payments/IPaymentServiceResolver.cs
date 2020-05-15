using System;
using System.Collections.Generic;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentServiceResolver
    {
        bool TryRegisterProvider(string paymentMethod, Type providerType);
        
        List<string> GetPaymentMethods();

        Type GetProviderTypeOrDefault(string paymentMethod);
    }
}