using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public interface IAccountRepository : IRepository<Account, Guid>
    {
        Task<Account> GetAsync(Guid userId, string accountGroupName, bool includeDetails = true,
            CancellationToken cancellationToken = default);
    }
}