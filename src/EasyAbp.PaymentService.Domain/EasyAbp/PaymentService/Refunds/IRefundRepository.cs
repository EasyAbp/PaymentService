using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.Refunds
{
    public interface IRefundRepository : IRepository<Refund, Guid>
    {
    }
}