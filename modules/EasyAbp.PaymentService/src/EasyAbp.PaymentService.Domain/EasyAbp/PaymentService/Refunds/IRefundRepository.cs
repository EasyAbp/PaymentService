using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.Refunds
{
    public interface IRefundRepository : IRepository<Refund, Guid>
    {
        Task<Refund> GetOngoingRefundOrNullAsync(Guid paymentId, CancellationToken cancellationToken = default);
    }
}