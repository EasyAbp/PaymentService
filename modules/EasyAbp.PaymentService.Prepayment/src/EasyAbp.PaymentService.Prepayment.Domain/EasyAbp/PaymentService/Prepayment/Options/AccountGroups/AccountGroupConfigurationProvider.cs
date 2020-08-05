using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.PaymentService.Prepayment.Options.AccountGroups
{
    public class AccountGroupConfigurationProvider : IAccountGroupConfigurationProvider, ITransientDependency
    {
        private readonly PaymentServicePrepaymentOptions _options;

        public AccountGroupConfigurationProvider(IOptions<PaymentServicePrepaymentOptions> options)
        {
            _options = options.Value;
        }
        
        public AccountGroupConfiguration Get(string accountGroupName)
        {
            return _options.AccountGroups.GetConfiguration(accountGroupName);
        }
    }
}