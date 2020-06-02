using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.WeChatPay.RefundRecords
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

        protected RefundRecord()
        {
        }

        public RefundRecord(
            Guid id,
            Guid? tenantId,
            Guid paymentId,
            string returnCode,
            string returnMsg,
            string appId,
            string mchId,
            string nonceStr,
            string reqInfo,
            string transactionId,
            string outTradeNo,
            string refundId,
            string outRefundNo,
            int totalFee,
            int? settlementTotalFee,
            int refundFee,
            int settlementRefundFee,
            string refundStatus,
            string successTime,
            string refundRecvAccout,
            string refundAccount,
            string refundRequestSource
        ) :base(id)
        {
            TenantId = tenantId;
            PaymentId = paymentId;
            ReturnCode = returnCode;
            ReturnMsg = returnMsg;
            AppId = appId;
            MchId = mchId;
            NonceStr = nonceStr;
            ReqInfo = reqInfo;
            TransactionId = transactionId;
            OutTradeNo = outTradeNo;
            RefundId = refundId;
            OutRefundNo = outRefundNo;
            TotalFee = totalFee;
            SettlementTotalFee = settlementTotalFee;
            RefundFee = refundFee;
            SettlementRefundFee = settlementRefundFee;
            RefundStatus = refundStatus;
            SuccessTime = successTime;
            RefundRecvAccout = refundRecvAccout;
            RefundAccount = refundAccount;
            RefundRequestSource = refundRequestSource;
        }
    }
}
