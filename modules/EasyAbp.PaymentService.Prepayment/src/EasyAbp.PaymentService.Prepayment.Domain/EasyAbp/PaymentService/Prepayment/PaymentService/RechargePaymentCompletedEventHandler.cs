using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Prepayment.Accounts;
using EasyAbp.PaymentService.Prepayment.Options.AccountGroups;
using EasyAbp.PaymentService.Prepayment.Transactions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.Prepayment.PaymentService
{
    public class RechargePaymentCompletedEventHandler : IDistributedEventHandler<PaymentCompletedEto>, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountGroupConfigurationProvider _accountGroupConfigurationProvider;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrentTenant _currentTenant;

        public RechargePaymentCompletedEventHandler(
            IGuidGenerator guidGenerator,
            ITransactionRepository transactionRepository,
            IAccountGroupConfigurationProvider accountGroupConfigurationProvider,
            IAccountRepository accountRepository,
            ICurrentTenant currentTenant)
        {
            _guidGenerator = guidGenerator;
            _transactionRepository = transactionRepository;
            _accountGroupConfigurationProvider = accountGroupConfigurationProvider;
            _accountRepository = accountRepository;
            _currentTenant = currentTenant;
        }
        
        [UnitOfWork(true)]
        public virtual async Task HandleEventAsync(PaymentCompletedEto eventData)
        {
            var payment = eventData.Payment;

            using var currentTenant = _currentTenant.Change(payment.TenantId);

            foreach (var item in payment.PaymentItems.Where(item => item.ItemType == PrepaymentConsts.RechargePaymentItemType))
            {
                var changedBalance = GetChangedBalance(item);
                
                var account = await _accountRepository.GetAsync(item.ItemKey);

                var configuration = _accountGroupConfigurationProvider.Get(account.AccountGroupName);

                var transaction = new Transaction(_guidGenerator.Create(), _currentTenant.Id, account.Id,
                    account.UserId, null, TransactionType.Debit, PrepaymentConsts.RechargeActionName,
                    payment.PaymentMethod, payment.ExternalTradingCode, configuration.Currency, changedBalance,
                    account.Balance);

                await _transactionRepository.InsertAsync(transaction, true);
                
                if (item.Currency != _accountGroupConfigurationProvider.Get(account.AccountGroupName).Currency)
                {
                    throw new CurrencyNotSupportedException(payment.Currency);
                }
                
                account.ChangeBalance(changedBalance);
                account.SetPendingRechargePaymentId(null);

                await _accountRepository.UpdateAsync(account, true);
            }
        }

        protected virtual decimal GetChangedBalance(PaymentItemEto paymentItem)
        {
            var changedBalance = paymentItem.OriginalPaymentAmount;

            if (!changedBalance.IsBetween(decimal.Zero, PrepaymentConsts.AccountMaxBalance))
            {
                throw new AmountOverflowException(decimal.Zero, PrepaymentConsts.AccountMaxBalance);
            }
            
            return changedBalance;
        }
    }
}