using System;
using System.Linq;
using System.Threading.Tasks;
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

        public virtual async Task<Payment> FindPaymentInProgressByPaymentItem(string paymentItemType, Guid paymentItemKey)
        {
            return await base.WithDetails()
                .Where(payment => !payment.CompletionTime.HasValue && !payment.CanceledTime.HasValue)
                .FirstOrDefaultAsync(payment =>
                    payment.PaymentItems.Any(item =>
                        item.ItemType == paymentItemType && item.ItemKey == paymentItemKey));
        }
    }
}