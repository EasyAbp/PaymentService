using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Accounts;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace PaymentServiceSample.Web
{
    public class NullAccountWithdrawalProvider : AccountWithdrawalProvider, ITransientDependency
    {
        public NullAccountWithdrawalProvider(IAccountWithdrawalManager accountWithdrawalManager) : base(
            accountWithdrawalManager)
        {
        }

        public override async Task OnStartWithdrawalAsync(Account account, string withdrawalMethod, decimal amount,
            ExtraPropertyDictionary inputExtraProperties)
        {
            await AccountWithdrawalManager.CompleteWithdrawalAsync(account);
        }

        public override Task OnCompleteWithdrawalAsync(Account account)
        {
            return Task.CompletedTask;
        }

        public override Task OnCancelWithdrawalAsync(Account account)
        {
            return Task.CompletedTask;
        }
    }
}