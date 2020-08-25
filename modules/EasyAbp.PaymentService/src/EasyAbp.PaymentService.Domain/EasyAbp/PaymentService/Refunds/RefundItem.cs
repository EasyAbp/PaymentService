using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Refunds
{
    public class RefundItem : FullAuditedEntity<Guid>, IRefundItemEntity
    {
        public virtual Guid PaymentItemId { get; protected set; }
        
        public virtual decimal RefundAmount { get; protected set; }
        
        [CanBeNull]
        public virtual string CustomerRemark { get; protected set; }
        
        [CanBeNull]
        public virtual string StaffRemark { get; protected set; }

        protected RefundItem()
        {
        }

        public RefundItem(
            Guid id,
            Guid paymentItemId,
            decimal refundAmount,
            [CanBeNull] string customerRemark,
            [CanBeNull] string staffRemark) : base(id)
        {
            PaymentItemId = paymentItemId;
            RefundAmount = refundAmount;
            CustomerRemark = customerRemark;
            StaffRemark = staffRemark;
        }
    }
}
