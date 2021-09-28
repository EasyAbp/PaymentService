using System;
using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class Account : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        
        [NotNull]
        public virtual string AccountGroupName { get; protected set; }
        
        public virtual Guid UserId { get; protected set; }
        
        public virtual decimal Balance { get; protected set; }
        
        public virtual decimal LockedBalance { get; protected set; }
        
        public virtual Guid? PendingTopUpPaymentId { get; protected set; }
        
        public virtual Guid? PendingWithdrawalRecordId { get; protected set; }
        
        public virtual decimal PendingWithdrawalAmount { get; protected set; }

        protected Account()
        {
        }

        public Account(Guid id,
            Guid? tenantId,
            [NotNull] string accountGroupName,
            Guid userId,
            decimal balance,
            decimal lockedBalance) : base(id)
        {
            TenantId = tenantId;
            AccountGroupName = accountGroupName;
            UserId = userId;
            Balance = balance;
            LockedBalance = lockedBalance;
        }

        public void ChangeBalance(decimal changedBalance)
        {
            var newBalance = decimal.Add(Balance, changedBalance);

            if (!newBalance.IsBetween(PrepaymentConsts.AccountMinBalance, PrepaymentConsts.AccountMaxBalance))
            {
                throw new AmountOverflowException(PrepaymentConsts.AccountMinBalance,
                    PrepaymentConsts.AccountMaxBalance);
            }
            
            if (newBalance < LockedBalance)
            {
                throw new LockedBalanceIsGreaterThenBalanceException(LockedBalance, newBalance);
            }

            Balance = newBalance;
        }
        
        public void ChangeLockedBalance(decimal changedLockedBalance, bool ignorePendingWithdrawalAmount = false)
        {
            var newLockedBalance = decimal.Add(LockedBalance, changedLockedBalance);
            
            if (Balance < newLockedBalance)
            {
                throw new LockedBalanceIsGreaterThenBalanceException(newLockedBalance, Balance);
            }

            if (!newLockedBalance.IsBetween(PrepaymentConsts.AccountMinLockedBalance,
                PrepaymentConsts.AccountMaxLockedBalance))
            {
                throw new AmountOverflowException(PrepaymentConsts.AccountMinLockedBalance,
                    PrepaymentConsts.AccountMaxLockedBalance);
            }
            
            if (!ignorePendingWithdrawalAmount)
            {
                var pendingWithdrawalAmount = PendingWithdrawalAmount;
                
                if (newLockedBalance < pendingWithdrawalAmount)
                {
                    throw new LockedBalanceIsLessThenPendingWithdrawalAmountException(newLockedBalance,
                        pendingWithdrawalAmount);
                }
            }

            LockedBalance = newLockedBalance;
        }

        public void SetPendingTopUpPaymentId(Guid? pendingTopUpPaymentId)
        {
            PendingTopUpPaymentId = pendingTopUpPaymentId;
        }

        public void StartWithdrawal(Guid pendingWithdrawalRecordId, decimal amount)
        {
            if (PendingWithdrawalRecordId.HasValue || PendingWithdrawalAmount != decimal.Zero)
            {
                throw new WithdrawalIsAlreadyInProgressException();
            }
            
            ChangeLockedBalance(amount);

            SetPendingWithdrawalRecordId(pendingWithdrawalRecordId);
            SetPendingWithdrawalAmount(amount);
        }
        
        public void CompleteWithdrawal()
        {
            ClearPendingWithdrawal();
            
            ChangeBalance(-PendingWithdrawalAmount);
        }
        
        public void CancelWithdrawal()
        {
            ClearPendingWithdrawal();
        }

        private void ClearPendingWithdrawal()
        {
            if (!PendingWithdrawalRecordId.HasValue || PendingWithdrawalAmount == decimal.Zero)
            {
                throw new WithdrawalInProgressNotFoundException();
            }
            

            ChangeLockedBalance(-PendingWithdrawalAmount, true);

            SetPendingWithdrawalRecordId(null);
            SetPendingWithdrawalAmount(0m);
        }
        
        private void SetPendingWithdrawalRecordId(Guid? pendingWithdrawalRecordId)
        {
            PendingWithdrawalRecordId = pendingWithdrawalRecordId;
        }
        
        private void SetPendingWithdrawalAmount(decimal pendingWithdrawalAmount)
        {
            PendingWithdrawalAmount = pendingWithdrawalAmount;
        }
    }
}
