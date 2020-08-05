using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.PaymentService.Prepayment.Options.WithdrawalMethods
{
    public class WithdrawalMethodConfigurationProvider : IWithdrawalMethodConfigurationProvider, ITransientDependency
    {
        private readonly PaymentServicePrepaymentOptions _options;

        public WithdrawalMethodConfigurationProvider(IOptions<PaymentServicePrepaymentOptions> options)
        {
            _options = options.Value;
        }
        
        public WithdrawalMethodConfiguration Get(string withdrawalMethodName)
        {
            return _options.WithdrawalMethods.GetConfiguration(withdrawalMethodName);
        }
    }
}