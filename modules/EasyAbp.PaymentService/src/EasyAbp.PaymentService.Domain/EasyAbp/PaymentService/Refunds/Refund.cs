using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Refunds
{
    public class Refund : FullAuditedAggregateRoot<Guid>, IRefundEntity
    {
        public virtual Guid? TenantId { get; protected set; }
        
        public virtual Guid PaymentId { get; protected set; }
        
        [NotNull]
        public virtual string RefundPaymentMethod { get; protected set; }
        
        [CanBeNull]
        public virtual string ExternalTradingCode { get; protected set; }
        
        [NotNull]
        public virtual string Currency { get; protected set; }
        
        public virtual decimal RefundAmount { get; protected set; }

        [CanBeNull]
        public virtual string DisplayReason { get; protected set; }

        [CanBeNull]
        public virtual string CustomerRemark { get; protected set; }
        
        [CanBeNull]
        public virtual string StaffRemark { get; protected set; }
        
        public virtual DateTime? CompletedTime { get; protected set; }
        
        public virtual DateTime? CanceledTime { get; protected set; }

        public virtual List<RefundItem> RefundItems { get; protected set; }

        protected Refund()
        {
        }

        public Refund(
            Guid id,
            Guid? tenantId,
            Guid paymentId,
            [NotNull] string refundPaymentMethod,
            [CanBeNull] string externalTradingCode,
            [NotNull] string currency,
            decimal refundAmount,
            [CanBeNull] string displayReason,
            [CanBeNull] string customerRemark,
            [CanBeNull] string staffRemark,
            List<RefundItem> refundItems
        ) : base(id)
        {
            TenantId = tenantId;
            PaymentId = paymentId;
            RefundPaymentMethod = refundPaymentMethod;
            ExternalTradingCode = externalTradingCode;
            Currency = currency;
            RefundAmount = refundAmount;
            DisplayReason = displayReason;
            CustomerRemark = customerRemark;
            StaffRemark = staffRemark;
            RefundItems = refundItems;
        }

        public void SetExternalTradingCode(string externalTradingCode)
        {
            ExternalTradingCode = externalTradingCode;
        }

        public void CompleteRefund(DateTime completedTime)
        {
            if (CompletedTime.HasValue || CanceledTime.HasValue)
            {
                throw new RefundIsInUnexpectedStageException(Id);
            }
            
            CompletedTime = completedTime;
        }

        public void CancelRefund(DateTime cancelTime)
        {
            if (CompletedTime.HasValue || CanceledTime.HasValue)
            {
                throw new RefundIsInUnexpectedStageException(Id);
            }

            CanceledTime = cancelTime;
        }
    }
}
