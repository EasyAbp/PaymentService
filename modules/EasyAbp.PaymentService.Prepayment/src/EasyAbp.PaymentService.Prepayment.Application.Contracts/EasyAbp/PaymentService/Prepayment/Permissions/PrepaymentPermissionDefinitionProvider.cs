using EasyAbp.PaymentService.Prepayment.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EasyAbp.PaymentService.Prepayment.Permissions
{
    public class PrepaymentPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(PrepaymentPermissions.GroupName, L("Permission:Prepayment"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PrepaymentResource>(name);
        }
    }
}