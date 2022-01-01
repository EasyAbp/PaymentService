using System;
using System.Collections.Generic;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentServiceResolver
    {
        List<string> GetPaymentMethods();

        Type GetProviderTypeOrDefault(string paymentMethod);
    }
}