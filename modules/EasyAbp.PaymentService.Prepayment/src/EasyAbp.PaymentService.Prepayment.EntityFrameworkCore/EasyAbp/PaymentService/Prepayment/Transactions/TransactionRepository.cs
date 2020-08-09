using System;
using EasyAbp.PaymentService.Prepayment.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Prepayment.Transactions
{
    public class TransactionRepository : EfCoreRepository<IPrepaymentDbContext, Transaction, Guid>, ITransactionRepository
    {
        public TransactionRepository(IDbContextProvider<PrepaymentDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}