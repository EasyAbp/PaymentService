using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Prepayment.Permissions;
using EasyAbp.PaymentService.Prepayment.Accounts.Dtos;
using EasyAbp.PaymentService.Prepayment.Options;
using EasyAbp.PaymentService.Prepayment.Options.AccountGroups;
using EasyAbp.PaymentService.Prepayment.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class AccountAppService : ReadOnlyAppService<Account, AccountDto, Guid, GetAccountListInput>,
        IAccountAppService
    {
        protected override string GetPolicyName { get; set; } = PrepaymentPermissions.Account.Default;
        protected override string GetListPolicyName { get; set; } = PrepaymentPermissions.Account.Default;

        private readonly PaymentServicePrepaymentOptions _options;
        private readonly IAccountGroupConfigurationProvider _accountGroupConfigurationProvider;
        private readonly IDistributedEventBus _distributedEventBus;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _repository;
        
        public AccountAppService(
            IAccountGroupConfigurationProvider accountGroupConfigurationProvider,
            IOptions<PaymentServicePrepaymentOptions> options,
            IDistributedEventBus distributedEventBus,
            ITransactionRepository transactionRepository,
            IAccountRepository repository) : base(repository)
        {
            _options = options.Value;
            _accountGroupConfigurationProvider = accountGroupConfigurationProvider;
            _distributedEventBus = distributedEventBus;
            _transactionRepository = transactionRepository;
            _repository = repository;
        }

        public override async Task<AccountDto> GetAsync(Guid id)
        {
            var dto = await base.GetAsync(id);

            if (dto.UserId != CurrentUser.GetId())
            {
                await AuthorizationService.CheckAsync(PrepaymentPermissions.Account.Manage);
            }

            return dto;
        }

        protected override IQueryable<Account> CreateFilteredQuery(GetAccountListInput input)
        {
            return base.CreateFilteredQuery(input).WhereIf(input.UserId.HasValue, x => x.UserId == input.UserId.Value);
        }

        public override async Task<PagedResultDto<AccountDto>> GetListAsync(GetAccountListInput input)
        {
            if (input.UserId != CurrentUser.GetId())
            {
                await AuthorizationService.CheckAsync(PrepaymentPermissions.Account.Manage);
            }

            var result = await base.GetListAsync(input);

            if (!input.UserId.HasValue)
            {
                return result;
            }

            var allAccountGroupNames = _options.AccountGroups.GetAutoCreationAccountGroupNames();
                
            var missingAccountGroupNames =
                allAccountGroupNames.Except(result.Items.Select(x => x.AccountGroupName)).ToArray();

            foreach (var accountGroupName in missingAccountGroupNames)
            {
                await _repository.InsertAsync(new Account(GuidGenerator.Create(), CurrentTenant.Id,
                    accountGroupName, input.UserId.Value, 0, 0), true);
            }

            if (!missingAccountGroupNames.IsNullOrEmpty())
            {
                result = await base.GetListAsync(input);
            }

            return result;
        }

        [Authorize(PrepaymentPermissions.Account.Manage)]
        public virtual async Task<AccountDto> ChangeBalanceAsync(ChangeBalanceInput input)
        {
            var account = await _repository.GetAsync(input.AccountId);

            var transactionType = input.ChangedBalance > decimal.Zero ? TransactionType.Debit : TransactionType.Credit;

            var transaction = new Transaction(GuidGenerator.Create(), CurrentTenant.Id, account.Id, account.UserId,
                null, transactionType, PrepaymentConsts.ChangeBalanceActionName,
                PrepaymentConsts.ChangeBalancePaymentMethod, null, null, input.ChangedBalance, account.Balance);

            await _transactionRepository.InsertAsync(transaction, true);
            
            account.ChangeBalance(input.ChangedBalance);

            await _repository.UpdateAsync(account, true);
            
            return MapToGetOutputDto(account);
        }

        [Authorize(PrepaymentPermissions.Account.Manage)]
        public virtual async Task<AccountDto> ChangeLockedBalanceAsync(ChangeLockedBalanceInput input)
        {
            var account = await _repository.GetAsync(input.AccountId);

            account.ChangeLockedBalance(input.ChangedLockedBalance);

            await _repository.UpdateAsync(account, true);
            
            return MapToGetOutputDto(account);
        }

        [Authorize(PrepaymentPermissions.Account.Recharge)]
        public virtual async Task RechargeAsync(RechargeInput input)
        {
            var account = await _repository.GetAsync(input.AccountId);

            if (account.UserId != CurrentUser.GetId())
            {
                throw new UnauthorizedRechargeException(account.Id);
            }
            
            var extraProperties = new Dictionary<string, object>();

            var configuration = _accountGroupConfigurationProvider.Get(account.AccountGroupName);
            
            await _distributedEventBus.PublishAsync(new CreatePaymentEto
            {
                TenantId = CurrentTenant.Id,
                UserId = CurrentUser.GetId(),
                PaymentMethod = input.PaymentMethod,
                Currency = configuration.Currency,
                ExtraProperties = extraProperties,
                PaymentItems = new List<CreatePaymentItemEto>(new []{new CreatePaymentItemEto
                {
                    ItemType = PrepaymentConsts.RechargePaymentItemType,
                    ItemKey = account.Id,
                    Currency = configuration.Currency,
                    OriginalPaymentAmount = input.Amount
                }})
            });
        }
    }
}
