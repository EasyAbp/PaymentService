using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentItem : FullAuditedEntity<Guid>, IPaymentItem
    {
        [NotNull]
        public virtual string ItemType { get; protected set; }
        
        public virtual string ItemKey { get; protected set; }

        public virtual decimal OriginalPaymentAmount { get; protected set; }

        public virtual decimal PaymentDiscount { get; protected set; }
        
        public virtual decimal ActualPaymentAmount { get; protected set; }
        
        public virtual decimal RefundAmount { get; protected set; }
        
        public virtual decimal PendingRefundAmount { get; protected set; }

        public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

        protected PaymentItem()
        {
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }

        public PaymentItem(
            Guid id,
            [NotNull] string itemType,
            [NotNull] string itemKey,
            decimal originalPaymentAmount
        ) :base(id)
        {
            ItemType = itemType;
            ItemKey = itemKey;
            OriginalPaymentAmount = originalPaymentAmount;
            ActualPaymentAmount = originalPaymentAmount;

            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }
        
        internal void SetPaymentDiscount(decimal paymentDiscount)
        {
            // Todo: ActualPaymentAmount should greater than 0
            PaymentDiscount = paymentDiscount;
            ActualPaymentAmount = OriginalPaymentAmount - paymentDiscount;
        }

        internal bool TryStartRefund(decimal refundAmount)
        {
            if (refundAmount <= decimal.Zero || ActualPaymentAmount < RefundAmount + refundAmount)
            {
                return false;
            }

            PendingRefundAmount = refundAmount;

            return true;
        }
        
        internal bool TryCompleteRefund()
        {
            if (PendingRefundAmount <= decimal.Zero)
            {
                return false;
            }

            RefundAmount += PendingRefundAmount;

            PendingRefundAmount = decimal.Zero;

            return true;
        }
        
        internal bool TryRollbackRefund()
        {
            if (PendingRefundAmount <= decimal.Zero)
            {
                return false;
            }

            PendingRefundAmount = decimal.Zero;

            return true;
        }
    }
}
