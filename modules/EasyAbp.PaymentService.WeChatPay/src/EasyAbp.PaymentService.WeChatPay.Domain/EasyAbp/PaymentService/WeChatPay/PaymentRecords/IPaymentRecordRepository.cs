using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.WeChatPay.PaymentRecords
{
    public interface IPaymentRecordRepository : IRepository<PaymentRecord, Guid>
    {
        Task<PaymentRecord> GetByPaymentId(Guid paymentId, CancellationToken cancellationToken = default);
    }
}