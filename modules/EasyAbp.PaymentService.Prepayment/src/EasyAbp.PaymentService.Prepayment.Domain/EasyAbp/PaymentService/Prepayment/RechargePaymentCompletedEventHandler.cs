using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Prepayment.Accounts;
using EasyAbp.PaymentService.Prepayment.Transactions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.Prepayment
{
    public class RechargePaymentCompletedEventHandler : IDistributedEventHandler<PaymentCompletedEto>, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrentTenant _currentTenant;

        public RechargePaymentCompletedEventHandler(
            IGuidGenerator guidGenerator,
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            ICurrentTenant currentTenant)
        {
            _guidGenerator = guidGenerator;
            _transactionRepository = transactionRepository;
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
                
                if (!Guid.TryParse(eventData.Payment.GetProperty<string>("AccountId"), out var accountId))
                {
                    throw new ArgumentNullException("AccountId");
                }
                
                var account = await _accountRepository.GetAsync(accountId);

                var transaction = new Transaction(_guidGenerator.Create(), _currentTenant.Id, account.Id, account.UserId,
                    null, TransactionType.Debit, PrepaymentConsts.RechargeActionName,
                    payment.PaymentMethod, payment.ExternalTradingCode, changedBalance, account.Balance);

                await _transactionRepository.InsertAsync(transaction, true);
            
                account.ChangeBalance(changedBalance);

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