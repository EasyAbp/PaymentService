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

            var accountPermission = myGroup.AddPermission(PrepaymentPermissions.Account.Default, L("Permission:Account"));
            accountPermission.AddChild(PrepaymentPermissions.Account.Manage, L("Permission:Manage"));
            accountPermission.AddChild(PrepaymentPermissions.Account.TopUp, L("Permission:TopUp"));
            accountPermission.AddChild(PrepaymentPermissions.Account.Withdraw, L("Permission:Withdraw"));

            var transactionPermission = myGroup.AddPermission(PrepaymentPermissions.Transaction.Default, L("Permission:Transaction"));
            transactionPermission.AddChild(PrepaymentPermissions.Transaction.Manage, L("Permission:Manage"));

            var withdrawalRecordPermission = myGroup.AddPermission(PrepaymentPermissions.WithdrawalRecord.Default, L("Permission:WithdrawalRecord"));
            withdrawalRecordPermission.AddChild(PrepaymentPermissions.WithdrawalRecord.Manage, L("Permission:Manage"));

            var withdrawalRequestPermission = myGroup.AddPermission(PrepaymentPermissions.WithdrawalRequest.Default, L("Permission:WithdrawalRequest"));
            withdrawalRequestPermission.AddChild(PrepaymentPermissions.WithdrawalRequest.Manage, L("Permission:Manage"));
            withdrawalRequestPermission.AddChild(PrepaymentPermissions.WithdrawalRequest.Review, L("Permission:Review"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PrepaymentResource>(name);
        }
    }
}
