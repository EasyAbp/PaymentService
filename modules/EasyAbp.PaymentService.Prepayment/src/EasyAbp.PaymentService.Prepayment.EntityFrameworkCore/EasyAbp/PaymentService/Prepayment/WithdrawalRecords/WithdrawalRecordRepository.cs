using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRecords
{
    public class WithdrawalRecordRepository : EfCoreRepository<IPaymentServicePrepaymentDbContext, WithdrawalRecord, Guid>,
        IWithdrawalRecordRepository
    {
        public WithdrawalRecordRepository(IDbContextProvider<IPaymentServicePrepaymentDbContext> dbContextProvider) : base(
            dbContextProvider)
        {
        }

        public async Task<decimal> GetCompletedTotalAmountAsync(Guid accountId, DateTime beginTime, DateTime endTime)
        {
            return await DbSet
                .Where(x => x.AccountId == accountId && x.CompletionTime >= beginTime && x.CompletionTime <= endTime)
                .SumAsync(x => x.Amount);
        }
    }
}