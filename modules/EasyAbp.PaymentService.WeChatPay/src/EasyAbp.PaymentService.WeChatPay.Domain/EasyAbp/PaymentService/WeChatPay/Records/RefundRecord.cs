using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.WeChatPay.Records
{
    public class RefundRecord : CreationAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        
        public virtual Guid PaymentId { get; protected set; }
        
        [NotNull]
        public virtual string ReturnCode { get; protected set; }
        
        [CanBeNull]
        public virtual string ReturnMsg { get; protected set; }
        
        [NotNull]
        public virtual string AppId { get; protected set; }
        
        [NotNull]
        public virtual string MchId { get; protected set; }

        [NotNull]
        public virtual string NonceStr { get; protected set; }
        
        [NotNull]
        public virtual string ReqInfo { get; protected set; }
        
        [NotNull]
        public virtual string TransactionId { get; protected set; }
        
        [NotNull]
        public virtual string OutTradeNo { get; protected set; }
        
        [NotNull]
        public virtual string RefundId { get; protected set; }
        
        [NotNull]
        public virtual string OutRefundNo { get; protected set; }
        
        public virtual int TotalFee { get; protected set; }
        
        public virtual int? SettlementTotalFee { get; protected set; }
        
        public virtual int RefundFee { get; protected set; }
        
        public virtual int SettlementRefundFee { get; protected set; }
        
        [NotNull]
        public virtual string RefundStatus { get; protected set; }
        
        [CanBeNull]
        public virtual string SuccessTime { get; protected set; }
        
        /// <summary>
        /// It's really named "Accout" by stupid WeChat team!
        /// </summary>
        [NotNull]
        public virtual string RefundRecvAccout { get; protected set; }
        
        [NotNull]
        public virtual string RefundAccount { get; protected set; }
        
        [NotNull]
        public virtual string RefundRequestSource { get; protected set; }
    }
}