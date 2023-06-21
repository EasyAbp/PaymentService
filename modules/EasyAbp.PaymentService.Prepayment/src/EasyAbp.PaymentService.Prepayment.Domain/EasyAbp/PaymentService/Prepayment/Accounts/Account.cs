using System;
using EasyAbp.PaymentService.Prepayment.Options.AccountGroups;
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

        public void ChangeBalance(AccountGroupConfiguration config, decimal changedBalance)
        {
            var newBalance = decimal.Add(Balance, changedBalance);

            CheckBalanceValue(config, newBalance, LockedBalance);

            Balance = newBalance;
        }

        public void ChangeLockedBalance(AccountGroupConfiguration config, decimal changedLockedBalance,
            bool ignorePendingWithdrawalAmount = false)
        {
            var newLockedBalance = decimal.Add(LockedBalance, changedLockedBalance);

            CheckBalanceValue(config, Balance, newLockedBalance);

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

        private static void CheckBalanceValue(AccountGroupConfiguration config, decimal balance, decimal lockedBalance)
        {
            var minBalance = config.AccountMinBalance ?? PrepaymentConsts.AccountMinBalance;
            var maxBalance = config.AccountMaxBalance ?? PrepaymentConsts.AccountMaxBalance;

            if (!balance.IsBetween(minBalance, maxBalance))
            {
                throw new AmountOverflowException("balance", minBalance, maxBalance);
            }

            if (lockedBalance < decimal.Zero)
            {
                throw new AmountOverflowException("locked balance", decimal.Zero, maxBalance);
            }

            if (balance - minBalance < lockedBalance)
            {
                throw new InsufficientBalanceToLockException(lockedBalance, balance);
            }
        }

        public void SetPendingTopUpPaymentId(Guid? pendingTopUpPaymentId)
        {
            PendingTopUpPaymentId = pendingTopUpPaymentId;
        }

        public void StartWithdrawal(AccountGroupConfiguration config, Guid pendingWithdrawalRecordId, decimal amount)
        {
            if (PendingWithdrawalRecordId.HasValue || PendingWithdrawalAmount != decimal.Zero)
            {
                throw new WithdrawalIsAlreadyInProgressException();
            }

            ChangeLockedBalance(config, amount);

            SetPendingWithdrawalRecordId(pendingWithdrawalRecordId);
            SetPendingWithdrawalAmount(amount);
        }

        public void CompleteWithdrawal(AccountGroupConfiguration config)
        {
            var balanceToChange = -PendingWithdrawalAmount;

            ClearPendingWithdrawal(config);

            ChangeBalance(config, balanceToChange);
        }

        public void CancelWithdrawal(AccountGroupConfiguration config)
        {
            ClearPendingWithdrawal(config);
        }

        private void ClearPendingWithdrawal(AccountGroupConfiguration config)
        {
            if (!PendingWithdrawalRecordId.HasValue || PendingWithdrawalAmount == decimal.Zero)
            {
                throw new WithdrawalInProgressNotFoundException();
            }


            ChangeLockedBalance(config, -1 * PendingWithdrawalAmount, true);

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