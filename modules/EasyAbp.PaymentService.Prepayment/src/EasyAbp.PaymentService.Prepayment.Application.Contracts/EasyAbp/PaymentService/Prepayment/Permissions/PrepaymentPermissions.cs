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

        public class Account
        {
            public const string Default = GroupName + ".Account";
            public const string Manage = Default + ".Manage";
            public const string TopUp = Default + ".TopUp";
            public const string Withdraw = Default + ".Withdraw";
        }

        public class Transaction
        {
            public const string Default = GroupName + ".Transaction";
            public const string Manage = Default + ".Manage";
        }

        public class WithdrawalRecord
        {
            public const string Default = GroupName + ".WithdrawalRecord";
            public const string Manage = Default + ".Manage";
        }
    }
}
