using Volo.Abp.Reflection;

namespace EasyAbp.PaymentService.Prepayment.Permissions
{
    public class PrepaymentPermissions
    {
        public const string GroupName = "EasyAbp.PaymentService.Prepayment";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(PrepaymentPermissions));
        }
    }
}