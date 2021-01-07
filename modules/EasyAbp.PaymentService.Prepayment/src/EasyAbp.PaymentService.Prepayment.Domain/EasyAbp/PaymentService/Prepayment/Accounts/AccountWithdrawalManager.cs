using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Options.AccountGroups;
using EasyAbp.PaymentService.Prepayment.Options.WithdrawalMethods;
using EasyAbp.PaymentService.Prepayment.Transactions;
using EasyAbp.PaymentService.Prepayment.WithdrawalRecords;
using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class AccountWithdrawalManager : DomainService, IAccountWithdrawalManager
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWithdrawalRecordRepository _withdrawalRecordRepository;
        private readonly IAccountGroupConfigurationProvider _accountGroupConfigurationProvider;
        private readonly IWithdrawalMethodConfigurationProvider _withdrawalMethodConfigurationProvider;

        public AccountWithdrawalManager(
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IWithdrawalRecordRepository withdrawalRecordRepository,
            IAccountGroupConfigurationProvider accountGroupConfigurationProvider,
            IWithdrawalMethodConfigurationProvider withdrawalMethodConfigurationProvider)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _withdrawalRecordRepository = withdrawalRecordRepository;
            _accountGroupConfigurationProvider = accountGroupConfigurationProvider;
            _withdrawalMethodConfigurationProvider = withdrawalMethodConfigurationProvider;
        }

        public virtual async Task StartWithdrawalAsync(Account account, string withdrawalMethod, decimal amount,
            ExtraPropertyDictionary inputExtraProperties)
        {
            var withdrawalProvider = GetWithdrawalProvider(withdrawalMethod);

            await CheckDailyWithdrawalAmountAsync(account, withdrawalMethod, amount);

            var withdrawalRecord = new WithdrawalRecord(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                account.Id,
                withdrawalMethod,
                amount);

            account.StartWithdrawal(withdrawalRecord.Id, withdrawalRecord.Amount);

            await _withdrawalRecordRepository.InsertAsync(withdrawalRecord, true);

            await _accountRepository.UpdateAsync(account, true);

            await withdrawalProvider.OnStartWithdrawalAsync(account, withdrawalMethod, amount, inputExtraProperties);
        }

        private async Task CheckDailyWithdrawalAmountAsync(Account account, [NotNull] string withdrawalMethodName,
            decimal expectedWithdrawalAmount)
        {
            var withdrawalMethodConfiguration = _withdrawalMethodConfigurationProvider.Get(withdrawalMethodName);

            var dailyMaxAmount = withdrawalMethodConfiguration.DailyMaximumWithdrawalAmountEachAccount;

            if (!dailyMaxAmount.HasValue)
            {
                return;
            }

            var beginTime = Clock.Now.Date;
            var endTime = beginTime.AddDays(1).AddTicks(-1);

            var dailyAmount =
                await _withdrawalRecordRepository.GetCompletedTotalAmountAsync(account.Id, beginTime, endTime);

            if (dailyAmount + expectedWithdrawalAmount > dailyMaxAmount.Value)
            {
                throw new WithdrawalAmountExceedDailyLimitException();
            }
        }

        private IAccountWithdrawalProvider GetWithdrawalProvider(string withdrawalMethodName)
        {
            var providerType =
                _withdrawalMethodConfigurationProvider.Get(withdrawalMethodName)?.AccountWithdrawalProviderType ??
                throw new UnknownWithdrawalMethodException(withdrawalMethodName);

            return ServiceProvider.GetService(providerType) as IAccountWithdrawalProvider ??
                                     throw new UnknownWithdrawalMethodException(withdrawalMethodName);
        }

        public virtual async Task CompleteWithdrawalAsync(Account account)
        {
            var withdrawalRecordId = account.PendingWithdrawalRecordId;

            if (!withdrawalRecordId.HasValue)
            {
                throw new WithdrawalInProgressNotFoundException();
            }
            
            var withdrawalRecord = await _withdrawalRecordRepository.GetAsync(withdrawalRecordId.Value);

            var accountGroupConfiguration = _accountGroupConfigurationProvider.Get(account.AccountGroupName);
            
            var withdrawalProvider = GetWithdrawalProvider(withdrawalRecord.WithdrawalMethod);

            var originalBalance = account.Balance;

            account.CompleteWithdrawal();
            
            withdrawalRecord.Complete(Clock.Now);

            await _accountRepository.UpdateAsync(account, true);
            
            await _withdrawalRecordRepository.UpdateAsync(withdrawalRecord, true);

            var accountChangedBalance = -1 * withdrawalRecord.Amount;

            var transaction = new Transaction(GuidGenerator.Create(), CurrentTenant.Id, account.Id, account.UserId,
                null, TransactionType.Credit, PrepaymentConsts.WithdrawalActionName, withdrawalRecord.WithdrawalMethod,
                null, accountGroupConfiguration.Currency, accountChangedBalance, originalBalance);

            await _transactionRepository.InsertAsync(transaction, true);
            
            await withdrawalProvider.OnCompleteWithdrawalAsync(account);

        }

        public virtual async Task CancelWithdrawalAsync(Account account, string errorCode = null,
            string errorMessage = null)
        {
            var withdrawalRecordId = account.PendingWithdrawalRecordId;

            if (!withdrawalRecordId.HasValue)
            {
                throw new WithdrawalInProgressNotFoundException();
            }

            var withdrawalRecord = await _withdrawalRecordRepository.GetAsync(withdrawalRecordId.Value);

            var withdrawalProvider = GetWithdrawalProvider(withdrawalRecord.WithdrawalMethod);

            account.CancelWithdrawal();

            withdrawalRecord.Cancel(Clock.Now, errorCode, errorMessage);

            await _accountRepository.UpdateAsync(account, true);

            await _withdrawalRecordRepository.UpdateAsync(withdrawalRecord, true);

            await withdrawalProvider.OnCancelWithdrawalAsync(account);
        }
    }
}