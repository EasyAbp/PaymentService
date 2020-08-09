using System;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.WeChatPay.PaymentRecords
{
    public class PaymentRecordRepository : EfCoreRepository<IWeChatPayDbContext, PaymentRecord, Guid>, IPaymentRecordRepository
    {
        public PaymentRecordRepository(IDbContextProvider<IWeChatPayDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<PaymentRecord> GetByPaymentId(Guid paymentId, CancellationToken cancellationToken = default)
        {
            var entity = await WithDetails().FirstOrDefaultAsync(x => x.PaymentId == paymentId, cancellationToken);
            
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(PaymentRecord));
            }

            return entity;
        }
    }
}