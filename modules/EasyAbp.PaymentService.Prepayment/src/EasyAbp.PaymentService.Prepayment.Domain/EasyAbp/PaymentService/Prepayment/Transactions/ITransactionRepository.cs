using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.Prepayment.Transactions
{
    public interface ITransactionRepository : IRepository<Transaction, Guid>
    {
    }
}