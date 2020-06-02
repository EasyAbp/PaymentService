using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.WeChatPay.PaymentRecords
{
    public interface IPaymentRecordRepository : IRepository<PaymentRecord, Guid>
    {
    }
}