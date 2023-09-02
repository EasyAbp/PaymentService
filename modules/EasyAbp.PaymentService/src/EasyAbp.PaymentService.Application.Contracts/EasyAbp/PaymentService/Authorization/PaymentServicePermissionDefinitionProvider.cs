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
            payment.AddChild(PaymentServicePermissions.Payments.Manage, L("Permission:Manage"));
            payment.AddChild(PaymentServicePermissions.Payments.Create, L("Permission:Create"));
            
            var refund = moduleGroup.AddPermission(PaymentServicePermissions.Refunds.Default, L("Permission:Refund"));
            refund.AddChild(PaymentServicePermissions.Refunds.Manage, L("Permission:Manage"));
            refund.AddChild(PaymentServicePermissions.Refunds.Create, L("Permission:Create"));

            var withdrawalRecordPermission = moduleGroup.AddPermission(PaymentServicePermissions.WithdrawalRecord.Default, L("Permission:WithdrawalRecord"));
            withdrawalRecordPermission.AddChild(PaymentServicePermissions.WithdrawalRecord.Create, L("Permission:Create"));
            withdrawalRecordPermission.AddChild(PaymentServicePermissions.WithdrawalRecord.Update, L("Permission:Update"));
            withdrawalRecordPermission.AddChild(PaymentServicePermissions.WithdrawalRecord.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PaymentServiceResource>(name);
        }
    }
}
