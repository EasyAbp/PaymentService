using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Options.WithdrawalMethods
{
    public class WithdrawalMethodConfigurations
    {
        private readonly Dictionary<string, WithdrawalMethodConfiguration> _withdrawalMethods;
        
        public WithdrawalMethodConfigurations()
        {
            _withdrawalMethods = new Dictionary<string, WithdrawalMethodConfiguration>();
        }

        public WithdrawalMethodConfigurations Configure<TWithdrawalMethod>(
            Action<WithdrawalMethodConfiguration> configureAction)
        {
            return Configure(
                WithdrawalMethodNameAttribute.GetWithdrawalMethodName<TWithdrawalMethod>(),
                configureAction
            );
        }

        public WithdrawalMethodConfigurations Configure(
            [NotNull] string name,
            [NotNull] Action<WithdrawalMethodConfiguration> configureAction)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(configureAction, nameof(configureAction));

            configureAction(
                _withdrawalMethods.GetOrAdd(
                    name,
                    () => new WithdrawalMethodConfiguration()
                )
            );

            return this;
        }

        public WithdrawalMethodConfigurations ConfigureAll(Action<string, WithdrawalMethodConfiguration> configureAction)
        {
            foreach (var withdrawalMethod in _withdrawalMethods)
            {
                configureAction(withdrawalMethod.Key, withdrawalMethod.Value);
            }
            
            return this;
        }

        [NotNull]
        public WithdrawalMethodConfiguration GetConfiguration<TWithdrawalMethod>()
        {
            return GetConfiguration(WithdrawalMethodNameAttribute.GetWithdrawalMethodName<TWithdrawalMethod>());
        }

        [NotNull]
        public WithdrawalMethodConfiguration GetConfiguration([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return _withdrawalMethods.GetOrDefault(name);
        }
    }
}