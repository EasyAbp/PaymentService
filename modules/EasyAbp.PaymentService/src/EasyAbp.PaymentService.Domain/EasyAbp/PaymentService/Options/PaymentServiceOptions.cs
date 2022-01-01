using EasyAbp.PaymentService.Options.PaymentServiceProviders;

namespace EasyAbp.PaymentService.Options
{
    public class PaymentServiceOptions
    {
        public PaymentServiceProviderConfigurations Providers { get; }

        public PaymentServiceOptions()
        {
            Providers = new PaymentServiceProviderConfigurations();
        }
    }
}