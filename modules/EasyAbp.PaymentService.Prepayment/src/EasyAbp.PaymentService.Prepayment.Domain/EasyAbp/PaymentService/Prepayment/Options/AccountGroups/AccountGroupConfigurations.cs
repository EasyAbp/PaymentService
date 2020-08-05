using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Options.AccountGroups
{
    public class AccountGroupConfigurations
    {
        private readonly Dictionary<string, AccountGroupConfiguration> _accountGroups;
        
        public AccountGroupConfigurations()
        {
            _accountGroups = new Dictionary<string, AccountGroupConfiguration>();
        }

        public AccountGroupConfigurations Configure<TAccountGroup>(
            Action<AccountGroupConfiguration> configureAction)
        {
            return Configure(
                AccountGroupNameAttribute.GetAccountGroupName<TAccountGroup>(),
                configureAction
            );
        }

        public AccountGroupConfigurations Configure(
            [NotNull] string name,
            [NotNull] Action<AccountGroupConfiguration> configureAction)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(configureAction, nameof(configureAction));

            configureAction(
                _accountGroups.GetOrAdd(
                    name,
                    () => new AccountGroupConfiguration()
                )
            );

            return this;
        }

        public AccountGroupConfigurations ConfigureAll(Action<string, AccountGroupConfiguration> configureAction)
        {
            foreach (var accountGroup in _accountGroups)
            {
                configureAction(accountGroup.Key, accountGroup.Value);
            }
            
            return this;
        }

        [NotNull]
        public AccountGroupConfiguration GetConfiguration<TAccountGroup>()
        {
            return GetConfiguration(AccountGroupNameAttribute.GetAccountGroupName<TAccountGroup>());
        }
        
        [NotNull]
        public string[] GetAutoCreationAccountGroupNames()
        {
            return _accountGroups.Where(pair => !pair.Value.DisableAccountAutoCreation).Select(pair => pair.Key)
                .ToArray();
        }

        [NotNull]
        public AccountGroupConfiguration GetConfiguration([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return _accountGroups.GetOrDefault(name);
        }
    }
}