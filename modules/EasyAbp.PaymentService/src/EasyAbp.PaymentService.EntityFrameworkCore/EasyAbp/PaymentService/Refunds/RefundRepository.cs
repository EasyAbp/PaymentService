using System;
using EasyAbp.PaymentService.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Refunds
{
    public class RefundRepository : EfCoreRepository<PaymentServiceDbContext, Refund, Guid>, IRefundRepository
    {
        public RefundRepository(IDbContextProvider<PaymentServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}