using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRecords
{
    public interface IWithdrawalRecordRepository : IRepository<WithdrawalRecord, Guid>
    {
        Task<decimal> GetCompletedTotalAmountAsync(Guid accountId, DateTime beginTime, DateTime endTime);
    }
}