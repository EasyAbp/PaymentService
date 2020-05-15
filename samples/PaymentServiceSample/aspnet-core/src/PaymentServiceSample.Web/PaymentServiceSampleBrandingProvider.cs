using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace PaymentServiceSample.Web
{
    [Dependency(ReplaceServices = true)]
    public class PaymentServiceSampleBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "PaymentServiceSample";
    }
}
