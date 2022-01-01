using System;
using EasyAbp.PaymentService.Prepayment.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class AccountRepository : EfCoreRepository<IPaymentServicePrepaymentDbContext, Account, Guid>, IAccountRepository
    {
        public AccountRepository(IDbContextProvider<IPaymentServicePrepaymentDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}