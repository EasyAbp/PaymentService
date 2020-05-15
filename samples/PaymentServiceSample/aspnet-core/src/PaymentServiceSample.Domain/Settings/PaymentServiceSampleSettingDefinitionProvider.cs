using Volo.Abp.Settings;

namespace PaymentServiceSample.Settings
{
    public class PaymentServiceSampleSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(PaymentServiceSampleSettings.MySetting1));
        }
    }
}
