using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Prepayment.Accounts;
using EasyAbp.PaymentService.Prepayment.Options.AccountGroups;
using EasyAbp.PaymentService.Prepayment.Transactions;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.DependencyInjection;
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
        private readonly IPrepaymentPayeeAccountProvider _payeeAccountProvider;
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
            IPrepaymentPayeeAccountProvider payeeAccountProvider,
            ITransactionRepository transactionRepository,
            IAccountGroupConfigurationProvider accountGroupConfigurationProvider)
        {
            _guidGenerator = guidGenerator;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _paymentManager = paymentManager;
            _paymentRepository = paymentRepository;
            _accountRepository = accountRepository;
            _payeeAccountProvider = payeeAccountProvider;
            _transactionRepository = transactionRepository;
            _accountGroupConfigurationProvider = accountGroupConfigurationProvider;
        }

        public override async Task OnPaymentStartedAsync(Payment payment, Dictionary<string, object> configurations)
        {
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

            if (!accountGroupConfiguration.AllowedToRechargeOtherAccounts &&
                payment.PaymentItems.Any(x => x.ItemType == PrepaymentConsts.RechargePaymentItemType))
            {
                throw new AccountRechargingOtherAccountsIsNotAllowedException(account.AccountGroupName);
            }

            if (payment.PaymentItems.Any(x =>
                x.ItemType == PrepaymentConsts.RechargePaymentItemType && x.ItemKey == accountId))
            {
                throw new SelfRechargingException();
            }

            if (payment.Currency != accountGroupConfiguration.Currency)
            {
                throw new CurrencyNotSupportedException(payment.Currency);
            }

            payment.SetPayeeAccount(await _payeeAccountProvider.GetPayeeAccountAsync(account));

            var accountChangedBalance = -1 * payment.ActualPaymentAmount;
            
            var transaction = new Transaction(_guidGenerator.Create(), _currentTenant.Id, account.Id, account.UserId,
                payment.Id, TransactionType.Credit, PrepaymentConsts.PaymentActionName,
                payment.PaymentMethod, payment.ExternalTradingCode, accountChangedBalance, account.Balance);

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
            var account = await _payeeAccountProvider.GetAccountByPayeeAccountAsync(payment.PayeeAccount);

            var accountChangedBalance = payment.ActualPaymentAmount;

            var transaction = new Transaction(_guidGenerator.Create(), _currentTenant.Id, account.Id, account.UserId,
                payment.Id, TransactionType.Debit, PrepaymentConsts.RefundActionName, payment.PaymentMethod,
                payment.ExternalTradingCode, accountChangedBalance, account.Balance);

            await _transactionRepository.InsertAsync(transaction, true);

            account.ChangeBalance(accountChangedBalance);
            
            await _accountRepository.UpdateAsync(account, true);
            
            await _paymentManager.CompleteRefundAsync(payment, refunds);
        }
    }
}