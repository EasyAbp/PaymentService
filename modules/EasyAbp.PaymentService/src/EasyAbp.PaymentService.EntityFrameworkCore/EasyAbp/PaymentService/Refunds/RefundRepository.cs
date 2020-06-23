using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.PaymentService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Refunds
{
    public class RefundRepository : EfCoreRepository<PaymentServiceDbContext, Refund, Guid>, IRefundRepository
    {
        public RefundRepository(IDbContextProvider<PaymentServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<Refund> GetOngoingRefundOrNullAsync(Guid paymentId, CancellationToken cancellationToken = default)
        {
            return await WithDetails()
                .FirstOrDefaultAsync(
                    x => x.PaymentId == paymentId && !x.CancelledTime.HasValue && !x.CompletedTime.HasValue,
                    cancellationToken);
        }
    }
}