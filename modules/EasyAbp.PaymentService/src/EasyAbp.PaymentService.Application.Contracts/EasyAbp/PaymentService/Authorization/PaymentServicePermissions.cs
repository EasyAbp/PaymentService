using Volo.Abp.Reflection;

namespace EasyAbp.PaymentService.Authorization
{
    public class PaymentServicePermissions
    {
        public const string GroupName = "EasyAbp.PaymentService";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(PaymentServicePermissions));
        }

        public class Payments
        {
            public const string Default = GroupName + ".Payment";

            public class Manage
            {
                public const string ManageDefault = Default + ".Manage";
                public const string Cancel = ManageDefault + ".Cancel";
                public const string RollbackRefund = ManageDefault + ".RollbackRefund";
            }
        }

        public class Refunds
        {
            public const string Default = GroupName + ".Refund";
            public const string Manage = Default + ".Manage";
        }
    }
}