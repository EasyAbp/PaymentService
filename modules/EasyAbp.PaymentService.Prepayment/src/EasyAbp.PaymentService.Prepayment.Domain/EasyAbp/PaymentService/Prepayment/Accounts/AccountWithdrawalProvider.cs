using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public abstract class AccountWithdrawalProvider : IAccountWithdrawalProvider
    {
        protected IAccountWithdrawalManager AccountWithdrawalManager { get; }

        public AccountWithdrawalProvider(IAccountWithdrawalManager accountWithdrawalManager)
        {
            AccountWithdrawalManager = accountWithdrawalManager;
        }

        public abstract Task OnStartWithdrawalAsync(Account account, string withdrawalMethod, decimal amount,
            Dictionary<string, object> inputExtraProperties);

        public abstract Task OnCompleteWithdrawalAsync(Account account);

        public abstract Task OnCancelWithdrawalAsync(Account account);
    }
}