using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentRepository : EfCoreRepository<IPaymentServiceDbContext, Payment, Guid>, IPaymentRepository
    {
        public PaymentRepository(IDbContextProvider<IPaymentServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<Payment>> WithDetailsAsync()
        {
            return (await base.WithDetailsAsync()).Include(x => x.PaymentItems);
        }

        public virtual async Task<Payment> FindPaymentInProgressByPaymentItem(string paymentItemType, string paymentItemKey)
        {
            return await (await base.WithDetailsAsync())
                .Where(payment => !payment.CompletionTime.HasValue && !payment.CanceledTime.HasValue)
                .FirstOrDefaultAsync(payment =>
                    payment.PaymentItems.Any(item =>
                        item.ItemType == paymentItemType && item.ItemKey == paymentItemKey));
        }
    }
}