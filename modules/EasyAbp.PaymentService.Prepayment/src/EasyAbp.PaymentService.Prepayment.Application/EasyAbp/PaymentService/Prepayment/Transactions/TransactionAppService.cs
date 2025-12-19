using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Accounts;
using EasyAbp.PaymentService.Prepayment.Localization;
using EasyAbp.PaymentService.Prepayment.Permissions;
using EasyAbp.PaymentService.Prepayment.Transactions.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace EasyAbp.PaymentService.Prepayment.Transactions
{
    public class TransactionAppService : ReadOnlyAppService<Transaction, TransactionDto, Guid, GetTransactionListInput>,
        ITransactionAppService
    {
        protected override string GetPolicyName { get; set; } = PrepaymentPermissions.Transaction.Default;
        protected override string GetListPolicyName { get; set; } = PrepaymentPermissions.Transaction.Default;

        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _repository;
        
        public TransactionAppService(
            IAccountRepository accountRepository,
            ITransactionRepository repository) : base(repository)
        {
            _accountRepository = accountRepository;
            _repository = repository;

            LocalizationResource = typeof(PrepaymentResource);
            ObjectMapperContext = typeof(PaymentServicePrepaymentApplicationModule);
        }

        public override async Task<TransactionDto> GetAsync(Guid id)
        {
            var dto = await base.GetAsync(id);

            var account = await _accountRepository.GetAsync(dto.AccountId);

            if (account.UserId != CurrentUser.GetId())
            {
                await AuthorizationService.CheckAsync(PrepaymentPermissions.Transaction.Manage);
            }

            return dto;
        }

        [Authorize]
        public override async Task<PagedResultDto<TransactionDto>> GetListAsync(GetTransactionListInput input)
        {
            var account = await _accountRepository.GetAsync(input.AccountId);
            
            if (account.UserId != CurrentUser.GetId())
            {
                await AuthorizationService.CheckAsync(PrepaymentPermissions.Transaction.Manage);
            }
            
            return await base.GetListAsync(input);
        }

        protected override async Task<IQueryable<Transaction>> CreateFilteredQueryAsync(GetTransactionListInput input)
        {
            return (await base.CreateFilteredQueryAsync(input)).Where(x => x.AccountId == input.AccountId);
        }
    }
}
