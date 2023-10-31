using EasyAbp.PaymentService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EasyAbp.PaymentService.Authorization
{
    public class PaymentServicePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(PaymentServicePermissions.GroupName, L("Permission:PaymentService"));
            
            var payment = moduleGroup.AddPermission(PaymentServicePermissions.Payments.Default, L("Permission:Payment"));
            payment.AddChild(PaymentServicePermissions.Payments.Manage.ManageDefault, L("Permission:Manage"));
            payment.AddChild(PaymentServicePermissions.Payments.Manage.Cancel, L("Permission:Cancel"));
            payment.AddChild(PaymentServicePermissions.Payments.Manage.RollbackRefund, L("Permission:RollbackRefund"));
            
            var refund = moduleGroup.AddPermission(PaymentServicePermissions.Refunds.Default, L("Permission:Refund"));
            refund.AddChild(PaymentServicePermissions.Refunds.Manage, L("Permission:Manage"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PaymentServiceResource>(name);
        }
    }
}
