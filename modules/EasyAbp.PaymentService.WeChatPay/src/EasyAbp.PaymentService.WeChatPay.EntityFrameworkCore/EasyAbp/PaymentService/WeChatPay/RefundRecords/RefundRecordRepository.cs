using System;
using EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.WeChatPay.RefundRecords
{
    public class RefundRecordRepository : EfCoreRepository<IWeChatPayDbContext, RefundRecord, Guid>, IRefundRecordRepository
    {
        public RefundRecordRepository(IDbContextProvider<IWeChatPayDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}