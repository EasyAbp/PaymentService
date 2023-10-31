using EasyAbp.PaymentService.WeChatPay.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EasyAbp.PaymentService.WeChatPay.Authorization
{
    public class WeChatPayPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(WeChatPayPermissions.GroupName, L("Permission:PaymentServiceWeChatPay"));

            var paymentRecord = moduleGroup.AddPermission(WeChatPayPermissions.PaymentRecords.Default,
                L("Permission:PaymentRecord"));

            var refundRecord = moduleGroup.AddPermission(WeChatPayPermissions.RefundRecords.Default,
                L("Permission:RefundRecord"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<WeChatPayResource>(name);
        }
    }
}