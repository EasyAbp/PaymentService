using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Prepayment.Accounts;
using EasyAbp.PaymentService.Prepayment.Options.AccountGroups;
using EasyAbp.PaymentService.Prepayment.Transactions;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.Data;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace EasyAbp.PaymentService.Prepayment.PaymentService
{
    public class PrepaymentPaymentServiceProvider : PaymentServiceProvider
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountGroupConfigurationProvider _accountGroupConfigurationProvider;

        public static string PaymentMethod { get; } = "Prepayment";
        
        public PrepaymentPaymentServiceProvider(
            IGuidGenerator guidGenerator,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IPaymentManager paymentManager,
            IPaymentRepository paymentRepository,
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IAccountGroupConfigurationProvider accountGroupConfigurationProvider)
        {
            _guidGenerator = guidGenerator;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _paymentManager = paymentManager;
            _paymentRepository = paymentRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _accountGroupConfigurationProvider = accountGroupConfigurationProvider;
        }

        public override async Task OnPaymentStartedAsync(Payment payment, Dictionary<string, object> configurations)
        {
            if (payment.ActualPaymentAmount <= decimal.Zero)
            {
                throw new PaymentAmountInvalidException(payment.ActualPaymentAmount, PaymentMethod);
            }
            
            if (!Guid.TryParse(configurations.GetOrDefault("AccountId") as string, out var accountId))
            {
                throw new ArgumentNullException("AccountId");
            }

            var account = await _accountRepository.GetAsync(accountId);

            if (account.UserId != _currentUser.GetId())
            {
                throw new UserIsNotAccountOwnerException(_currentUser.GetId(), accountId);
            }

            var accountGroupConfiguration = _accountGroupConfigurationProvider.Get(account.AccountGroupName);

            if (!accountGroupConfiguration.AllowedUsingToTopUpOtherAccounts &&
                payment.PaymentItems.Any(x => x.ItemType == PrepaymentConsts.TopUpPaymentItemType))
            {
                throw new AccountTopingUpOtherAccountsIsNotAllowedException(account.AccountGroupName);
            }

            if (payment.PaymentItems.Any(x =>
                x.ItemType == PrepaymentConsts.TopUpPaymentItemType && x.ItemKey == accountId))
            {
                throw new SelfTopUpException();
            }

            if (payment.Currency != accountGroupConfiguration.Currency)
            {
                throw new CurrencyNotSupportedException(payment.Currency);
            }

            var accountChangedBalance = -1 * payment.ActualPaymentAmount;

            var transaction = new Transaction(_guidGenerator.Create(), _currentTenant.Id, account.Id, account.UserId,
                payment.Id, TransactionType.Credit, PrepaymentConsts.PaymentActionName, payment.PaymentMethod,
                payment.ExternalTradingCode, accountGroupConfiguration.Currency, accountChangedBalance,
                account.Balance);

            await _transactionRepository.InsertAsync(transaction, true);

            account.ChangeBalance(accountChangedBalance);
            
            await _accountRepository.UpdateAsync(account, true);
            
            await _paymentManager.CompletePaymentAsync(payment);

            await _paymentRepository.UpdateAsync(payment, true);
        }

        public override async Task OnCancelStartedAsync(Payment payment)
        {
            await _paymentManager.CompleteCancelAsync(payment);
        }

        public override async Task OnRefundStartedAsync(Payment payment, IEnumerable<Refund> refunds, string displayReason = null)
        {
            if (!Guid.TryParse(payment.GetProperty<string>("AccountId"), out var accountId))
            {
                throw new ArgumentNullException("AccountId");
            }
            
            var account = await _accountRepository.GetAsync(accountId);

            var configuration = _accountGroupConfigurationProvider.Get(account.AccountGroupName);

            var accountChangedBalance = payment.ActualPaymentAmount;

            var transaction = new Transaction(_guidGenerator.Create(), _currentTenant.Id, account.Id, account.UserId,
                payment.Id, TransactionType.Debit, PrepaymentConsts.RefundActionName, payment.PaymentMethod,
                payment.ExternalTradingCode, configuration.Currency, accountChangedBalance, account.Balance);

            await _transactionRepository.InsertAsync(transaction, true);

            account.ChangeBalance(accountChangedBalance);
            
            await _accountRepository.UpdateAsync(account, true);
            
            await _paymentManager.CompleteRefundAsync(payment, refunds);
        }
    }
}