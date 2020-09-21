using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRequests
{
    public interface IWithdrawalRequestRepository : IRepository<WithdrawalRequest, Guid>
    {
    }
}