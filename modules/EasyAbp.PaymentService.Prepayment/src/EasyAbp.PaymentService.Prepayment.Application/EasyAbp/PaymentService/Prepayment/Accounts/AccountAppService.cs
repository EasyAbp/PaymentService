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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
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
                await AuthorizationService.CheckAsync(PrepaymentPermissions.Account.Manage.ManageDefault);
            }

            return dto;
        }

        protected override async Task<IQueryable<Account>> CreateFilteredQueryAsync(GetAccountListInput input)
        {
            return (await base.CreateFilteredQueryAsync(input))
                .WhereIf(input.UserId.HasValue, x => x.UserId == input.UserId.Value);
        }

        [Authorize]
        public override async Task<PagedResultDto<AccountDto>> GetListAsync(GetAccountListInput input)
        {
            if (input.UserId != CurrentUser.GetId())
            {
                await AuthorizationService.CheckAsync(PrepaymentPermissions.Account.Manage.ManageDefault);
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

        [Authorize(PrepaymentPermissions.Account.Manage.ChangeBalance)]
        public virtual async Task<AccountDto> ChangeBalanceAsync(Guid id, ChangeBalanceInput input)
        {
            var account = await _repository.GetAsync(id);

            var configuration = _accountGroupConfigurationProvider.Get(account.AccountGroupName);

            var transactionType = input.ChangedBalance > decimal.Zero ? TransactionType.Debit : TransactionType.Credit;

            var transaction = new Transaction(GuidGenerator.Create(), CurrentTenant.Id, account.Id, account.UserId,
                null, transactionType, PrepaymentConsts.ChangeBalanceActionName,
                PrepaymentConsts.ChangeBalancePaymentMethod, null, configuration.Currency, input.ChangedBalance,
                account.Balance);

            await _transactionRepository.InsertAsync(transaction, true);

            account.ChangeBalance(configuration, input.ChangedBalance);

            await _repository.UpdateAsync(account, true);

            return await MapToGetOutputDtoAsync(account);
        }

        [Authorize(PrepaymentPermissions.Account.Manage.ChangeLockedBalance)]
        public virtual async Task<AccountDto> ChangeLockedBalanceAsync(Guid id, ChangeLockedBalanceInput input)
        {
            var account = await _repository.GetAsync(id);

            var configuration = _accountGroupConfigurationProvider.Get(account.AccountGroupName);

            account.ChangeLockedBalance(configuration, input.ChangedLockedBalance);

            await _repository.UpdateAsync(account, true);

            return await MapToGetOutputDtoAsync(account);
        }

        [Authorize(PrepaymentPermissions.Account.TopUp)]
        public virtual async Task TopUpAsync(Guid id, TopUpInput input)
        {
            var account = await _repository.GetAsync(id);

            if (account.UserId != CurrentUser.GetId())
            {
                throw new UnauthorizedTopUpException(account.Id);
            }

            if (account.PendingTopUpPaymentId.HasValue)
            {
                throw new TopUpIsAlreadyInProgressException();
            }

            var configuration = _accountGroupConfigurationProvider.Get(account.AccountGroupName);

            await _distributedEventBus.PublishAsync(new CreatePaymentEto(
                CurrentTenant.Id,
                CurrentUser.GetId(),
                input.PaymentMethod,
                configuration.Currency,
                new List<CreatePaymentItemEto>(new[]
                {
                    new CreatePaymentItemEto
                    {
                        ItemType = PrepaymentConsts.TopUpPaymentItemType,
                        ItemKey = account.Id.ToString(),
                        OriginalPaymentAmount = input.Amount
                    }
                })
            ));
        }

        [Authorize(PrepaymentPermissions.Account.Withdraw)]
        public virtual async Task WithdrawAsync(Guid id, WithdrawInput input)
        {
            var account = await _repository.GetAsync(id);

            if (account.UserId != CurrentUser.GetId())
            {
                throw new AbpAuthorizationException();
            }

            var accountWithdrawalManager = ServiceProvider.GetRequiredService<IAccountWithdrawalManager>();

            await accountWithdrawalManager.StartWithdrawalAsync(account, input.WithdrawalMethod, input.Amount,
                input.ExtraProperties);
        }
    }
}