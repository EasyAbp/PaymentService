using Volo.Abp.Settings;

namespace EasyAbp.PaymentService.Settings
{
    public class PaymentServiceSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            /* Define module settings here.
             * Use names from PaymentServiceSettings class.
             */

            context.Add(
                new SettingDefinition(PaymentServiceSettings.FreePaymentMethod.DefaultPayeeAccount, "None")
            );
        }
    }
}