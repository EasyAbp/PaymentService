using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRequests
{
    /// <summary>
    /// This entity will be created if a user requests to withdraw with the "Manual" withdrawal method.
    /// </summary>
    public class WithdrawalRequest : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        
        public virtual Guid AccountId { get; protected set; }
        
        public virtual Guid AccountUserId { get; protected set; }
        
        public virtual decimal Amount { get; protected set; }
        
        public virtual DateTime? ReviewTime { get; protected set; }
        
        public virtual Guid? ReviewerUserId { get; protected set; }
        
        public virtual bool? IsApproved { get; protected set; }

        protected WithdrawalRequest()
        {
        }

        public WithdrawalRequest(
            Guid id,
            Guid? tenantId,
            Guid accountId,
            Guid accountUserId,
            decimal amount
        ) : base(id)
        {
            TenantId = tenantId;
            AccountId = accountId;
            AccountUserId = accountUserId;
            Amount = amount;
        }

        public void Review(DateTime reviewTime, Guid reviewerUserId, bool isApproved)
        {
            CheckReviewable();

            ReviewTime = reviewTime;
            ReviewerUserId = reviewerUserId;
            IsApproved = isApproved;
        }

        public void CheckReviewable()
        {
            if (ReviewTime.HasValue || ReviewerUserId.HasValue || IsApproved.HasValue)
            {
                throw new WithdrawalRequestHasBeenReviewedException();
            }
        }
    }
}
