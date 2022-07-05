using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.WeChatPay.RefundRecords
{
    public class RefundRecordRepository : EfCoreRepository<IPaymentServiceWeChatPayDbContext, RefundRecord, Guid>,
        IRefundRecordRepository
    {
        public RefundRecordRepository(IDbContextProvider<IPaymentServiceWeChatPayDbContext> dbContextProvider) : base(
            dbContextProvider)
        {
        }

        public virtual async Task<RefundRecord> FindByOutRefundNoAsync(string outRefundNo)
        {
            return await (await WithDetailsAsync()).Where(x => x.OutRefundNo == outRefundNo).SingleOrDefaultAsync();
        }
    }
}