using System;
using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class Account : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        private const string PendingTopUpPaymentIdPropertyName = "PendingTopUpPaymentId";
        private const string PendingWithdrawalRecordIdPropertyName = "PendingWithdrawalRecordId";
        private const string PendingWithdrawalAmountPropertyName = "PendingWithdrawalAmount";
        
        public virtual Guid? TenantId { get; protected set; }
        
        [NotNull]
        public virtual string AccountGroupName { get; protected set; }
        
        public virtual Guid UserId { get; protected set; }
        
        public virtual decimal Balance { get; protected set; }
        
        public virtual decimal LockedBalance { get; protected set; }

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

            if (newBalance < LockedBalance)
            {
                throw new LockedBalanceIsGreaterThenBalanceException(LockedBalance, newBalance);
            }
            
            if (!newBalance.IsBetween(PrepaymentConsts.AccountMinBalance, PrepaymentConsts.AccountMaxBalance))
            {
                throw new AmountOverflowException(PrepaymentConsts.AccountMinBalance,
                    PrepaymentConsts.AccountMaxBalance);
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
                var pendingWithdrawalAmount = GetPendingWithdrawalAmount();
                
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
            if (pendingTopUpPaymentId.HasValue)
            {
                this.SetProperty(PendingTopUpPaymentIdPropertyName, pendingTopUpPaymentId.ToString());
            }
            else
            {
                this.RemoveProperty(PendingTopUpPaymentIdPropertyName);
            }
        }
        
        public Guid? GetPendingTopUpPaymentId()
        {
            if (Guid.TryParse(this.GetProperty<string>(PendingTopUpPaymentIdPropertyName), out var paymentId))
            {
                return paymentId;
            }

            return null;
        }
        
        public void StartWithdrawal(Guid pendingWithdrawalRecordId, decimal amount)
        {
            if (GetPendingWithdrawalRecordId().HasValue || GetPendingWithdrawalAmount() != decimal.Zero)
            {
                throw new WithdrawalIsAlreadyInProgressException();
            }
            
            ChangeLockedBalance(amount);

            SetPendingWithdrawalRecordId(pendingWithdrawalRecordId);
            SetPendingWithdrawalAmount(amount);
        }
        
        public void CompleteWithdrawal()
        {
            var pendingAmount = GetPendingWithdrawalAmount();

            ClearPendingWithdrawal();
            
            ChangeBalance(-pendingAmount);
        }
        
        public void CancelWithdrawal()
        {
            ClearPendingWithdrawal();
        }

        private void ClearPendingWithdrawal()
        {
            var pendingAmount = GetPendingWithdrawalAmount();

            if (!GetPendingWithdrawalRecordId().HasValue || pendingAmount == decimal.Zero)
            {
                throw new WithdrawalInProgressNotFoundException();
            }
            

            ChangeLockedBalance(-pendingAmount, true);

            SetPendingWithdrawalRecordId(null);
            SetPendingWithdrawalAmount(0m);
        }
        
        private void SetPendingWithdrawalRecordId(Guid? pendingWithdrawalRecordId)
        {
            if (pendingWithdrawalRecordId.HasValue)
            {
                this.SetProperty(PendingWithdrawalRecordIdPropertyName, pendingWithdrawalRecordId.ToString());
            }
            else
            {
                this.RemoveProperty(PendingWithdrawalRecordIdPropertyName);
            }
        }
        
        public Guid? GetPendingWithdrawalRecordId()
        {
            if (Guid.TryParse(this.GetProperty<string>(PendingWithdrawalRecordIdPropertyName), out var withdrawalRecordId))
            {
                return withdrawalRecordId;
            }

            return null;
        }
        
        private void SetPendingWithdrawalAmount(decimal pendingWithdrawalAmount)
        {
            this.SetProperty(PendingWithdrawalAmountPropertyName, pendingWithdrawalAmount);
        }
        
        public decimal GetPendingWithdrawalAmount()
        {
            return this.GetProperty<decimal>(PendingWithdrawalAmountPropertyName);
        }
    }
}
