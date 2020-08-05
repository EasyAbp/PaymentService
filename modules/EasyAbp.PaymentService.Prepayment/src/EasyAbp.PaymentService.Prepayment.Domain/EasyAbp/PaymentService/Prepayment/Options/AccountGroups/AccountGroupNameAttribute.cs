using System;
using System.Reflection;
using JetBrains.Annotations;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Options.AccountGroups
{
    public class AccountGroupNameAttribute : Attribute
    {
        [NotNull]
        public string Name { get; }

        public AccountGroupNameAttribute([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            Name = name;
        }

        public virtual string GetName(Type type)
        {
            return Name;
        }

        public static string GetAccountGroupName<T>()
        {
            return GetAccountGroupName(typeof(T));
        }

        public static string GetAccountGroupName(Type type)
        {
            var nameAttribute = type.GetCustomAttribute<AccountGroupNameAttribute>();

            if (nameAttribute == null)
            {
                return type.FullName;
            }

            return nameAttribute.GetName(type);
        }
    }
}