using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace PaymentServiceSample.Web
{
    [Dependency(ReplaceServices = true)]
    public class PaymentServiceSampleBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "PaymentServiceSample";
    }
}
