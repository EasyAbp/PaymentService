using System;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class AccountRepository : EfCoreRepository<PrepaymentDbContext, Account, Guid>, IAccountRepository
    {
        public AccountRepository(IDbContextProvider<PrepaymentDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<Account> GetAsync(Guid userId, string accountGroupName, bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetAsync(x => x.UserId == userId && x.AccountGroupName == accountGroupName, includeDetails,
                cancellationToken);
        }
    }
}