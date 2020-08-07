using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Prepayment.Transactions
{
    public class Transaction : CreationAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        
        public virtual Guid AccountId { get; protected set; }
        
        public virtual Guid AccountUserId { get; protected set; }
        
        public virtual Guid? PaymentId { get; protected set; }

        public virtual TransactionType TransactionType { get; protected set; }
        
        [NotNull]
        public virtual string ActionName { get; protected set; }
        
        [NotNull]
        public virtual string PaymentMethod { get; protected set; }
        
        [CanBeNull]
        public virtual string ExternalTradingCode { get; protected set; }
        
        [NotNull]
        public virtual string Currency { get; protected set; }
        
        public virtual decimal ChangedBalance { get; protected set; }
        
        public virtual decimal OriginalBalance { get; protected set; }

        protected Transaction()
        {
        }

        public Transaction(Guid id,
            Guid? tenantId,
            Guid accountId,
            Guid accountUserId,
            Guid? paymentId,
            TransactionType transactionType,
            [NotNull] string actionName,
            [NotNull] string paymentMethod,
            [CanBeNull] string externalTradingCode,
            [NotNull] string currency,
            decimal changedBalance,
            decimal originalBalance) : base(id)
        {
            TenantId = tenantId;
            AccountId = accountId;
            AccountUserId = accountUserId;
            PaymentId = paymentId;
            TransactionType = transactionType;
            ActionName = actionName;
            PaymentMethod = paymentMethod;
            ExternalTradingCode = externalTradingCode;
            Currency = currency;
            ChangedBalance = changedBalance;
            OriginalBalance = originalBalance;
        }
    }
}
