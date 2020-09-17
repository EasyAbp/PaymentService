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
            public const string Manage = Default + ".Manage";
            public const string CrossStore = Default + ".CrossStore";
            public const string Create = Default + ".Create";
        }
        
        public class Refunds
        {
            public const string Default = GroupName + ".Refund";
            public const string Manage = Default + ".Manage";
            public const string CrossStore = Default + ".CrossStore";
            public const string Create = Default + ".Create";
        }

        public class WithdrawalRecord
        {
            public const string Default = GroupName + ".WithdrawalRecord";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }
    }
}
