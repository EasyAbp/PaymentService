using System;
using EasyAbp.PaymentService.Prepayment.Permissions;
using EasyAbp.PaymentService.Prepayment.Transactions.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.Prepayment.Transactions
{
    public class TransactionAppService : ReadOnlyAppService<Transaction, TransactionDto, Guid, PagedAndSortedResultRequestDto>,
        ITransactionAppService
    {
        protected override string GetPolicyName { get; set; } = PrepaymentPermissions.Transaction.Default;
        protected override string GetListPolicyName { get; set; } = PrepaymentPermissions.Transaction.Default;

        private readonly ITransactionRepository _repository;
        
        public TransactionAppService(ITransactionRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
