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
        
        public virtual Guid UserId { get; protected set; }
        
        public virtual Guid? PaymentId { get; protected set; }

        public virtual TransactionType TransactionType { get; protected set; }
        
        [NotNull]
        public virtual string ActionName { get; protected set; }
        
        [NotNull]
        public virtual string PaymentMethod { get; protected set; }
        
        [CanBeNull]
        public virtual string OppositePartAccount { get; protected set; }
        
        [CanBeNull]
        public virtual string ExternalTradingCode { get; protected set; }
        
        public virtual decimal ChangedBalance { get; protected set; }
        
        public virtual decimal OriginalBalance { get; protected set; }
    }
}