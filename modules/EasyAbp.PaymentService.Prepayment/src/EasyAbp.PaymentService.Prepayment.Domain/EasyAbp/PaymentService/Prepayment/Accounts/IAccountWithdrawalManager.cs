using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Services;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public interface IAccountWithdrawalManager : IDomainService
    {
        Task StartWithdrawalAsync(Account account, [NotNull] string withdrawalMethod, decimal amount,
            Dictionary<string, object> inputExtraProperties);
        
        Task CompleteWithdrawalAsync(Account account);

        Task CancelWithdrawalAsync(Account account, [CanBeNull] string errorCode = null,
            [CanBeNull] string errorMessage = null);
    }
}