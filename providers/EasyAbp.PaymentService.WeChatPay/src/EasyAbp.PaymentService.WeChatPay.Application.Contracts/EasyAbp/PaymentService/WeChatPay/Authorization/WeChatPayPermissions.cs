using Volo.Abp.Reflection;

namespace EasyAbp.PaymentService.WeChatPay.Authorization
{
    public class WeChatPayPermissions
    {
        public const string GroupName = "EasyAbp.PaymentService.WeChatPay";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(WeChatPayPermissions));
        }
    }
}