using System;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;

namespace EasyAbp.PaymentService.Refunds
{
    [Serializable]
    public class RefundItem : FullAuditedEntity<Guid>, IRefundItem
    {
        public virtual Guid PaymentItemId { get; protected set; }
        
        public virtual decimal RefundAmount { get; protected set; }
        
        [CanBeNull]
        public virtual string CustomerRemark { get; protected set; }
        
        [CanBeNull]
        public virtual string StaffRemark { get; protected set; }
        
        [JsonInclude]
        public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }
        
        protected RefundItem()
        {
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
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
            
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }
    }
}
