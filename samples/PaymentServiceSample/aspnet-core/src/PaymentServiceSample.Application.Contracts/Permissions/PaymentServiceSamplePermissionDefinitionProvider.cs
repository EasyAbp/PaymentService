using PaymentServiceSample.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace PaymentServiceSample.Permissions
{
    public class PaymentServiceSamplePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(PaymentServiceSamplePermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(PaymentServiceSamplePermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PaymentServiceSampleResource>(name);
        }
    }
}
