using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public interface IAccountRepository : IRepository<Account, Guid>
    {
    }
}