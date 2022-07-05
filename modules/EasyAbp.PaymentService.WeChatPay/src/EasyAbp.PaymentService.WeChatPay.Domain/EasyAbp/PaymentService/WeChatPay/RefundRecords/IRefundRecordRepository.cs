using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.WeChatPay.RefundRecords
{
    public interface IRefundRecordRepository : IRepository<RefundRecord, Guid>
    {
        Task<RefundRecord> FindByOutRefundNoAsync([NotNull] string outRefundNo);
    }
}