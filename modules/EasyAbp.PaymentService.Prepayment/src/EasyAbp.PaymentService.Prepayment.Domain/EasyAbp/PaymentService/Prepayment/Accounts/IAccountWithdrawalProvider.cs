using System.Threading.Tasks;
using Volo.Abp.Data;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public interface IAccountWithdrawalProvider
    {
        Task OnStartWithdrawalAsync(Account account, string withdrawalMethod, decimal amount,
            ExtraPropertyDictionary inputExtraProperties);

        Task OnCompleteWithdrawalAsync(Account account);
        
        Task OnCancelWithdrawalAsync(Account account);
    }
}