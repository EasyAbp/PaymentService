using System;
using EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.WeChatPay.PaymentRecords
{
    public class PaymentRecordRepository : EfCoreRepository<WeChatPayDbContext, PaymentRecord, Guid>, IPaymentRecordRepository
    {
        public PaymentRecordRepository(IDbContextProvider<WeChatPayDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}