using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public interface IAccountWithdrawalProvider
    {
        Task OnStartWithdrawalAsync(Account account, string withdrawalMethod, decimal amount,
            Dictionary<string, object> inputExtraProperties);

        Task OnCompleteWithdrawalAsync(Account account);
        
        Task OnCancelWithdrawalAsync(Account account);
    }
}