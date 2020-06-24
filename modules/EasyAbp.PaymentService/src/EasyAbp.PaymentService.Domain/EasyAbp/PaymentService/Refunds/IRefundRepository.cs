using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.Refunds
{
    public interface IRefundRepository : IRepository<Refund, Guid>
    {
        Task<List<Refund>> GetOngoingRefundListAsync(Guid paymentId, CancellationToken cancellationToken = default);
    }
}