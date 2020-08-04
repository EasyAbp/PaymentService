using System;
using JetBrains.Annotations;
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
        
        public void ChangeLockedBalance(decimal changedLockedBalance)
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

            LockedBalance = newLockedBalance;
        }
    }
}
