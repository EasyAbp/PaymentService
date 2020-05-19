using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.WeChatPay.RefundRecords
{
    public interface IRefundRecordRepository : IRepository<RefundRecord, Guid>
    {
    }
}