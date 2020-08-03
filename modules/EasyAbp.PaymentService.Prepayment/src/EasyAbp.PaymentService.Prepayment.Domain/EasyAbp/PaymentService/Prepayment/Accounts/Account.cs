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
    }
}