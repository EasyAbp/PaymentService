using System;
using System.Linq;
using EasyAbp.PaymentService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentRepository : EfCoreRepository<PaymentServiceDbContext, Payment, Guid>, IPaymentRepository
    {
        public PaymentRepository(IDbContextProvider<PaymentServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override IQueryable<Payment> WithDetails()
        {
            return base.WithDetails().Include(x => x.PaymentItems);
        }
    }
}