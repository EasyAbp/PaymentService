using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.PaymentService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Refunds
{
    public class RefundRepository : EfCoreRepository<IPaymentServiceDbContext, Refund, Guid>, IRefundRepository
    {
        public RefundRepository(IDbContextProvider<PaymentServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<List<Refund>> GetOngoingRefundListAsync(Guid paymentId, CancellationToken cancellationToken = default)
        {
            return await WithDetails()
                .Where(x => x.PaymentId == paymentId && !x.CanceledTime.HasValue && !x.CompletedTime.HasValue)
                .ToListAsync(cancellationToken);
        }
    }
}