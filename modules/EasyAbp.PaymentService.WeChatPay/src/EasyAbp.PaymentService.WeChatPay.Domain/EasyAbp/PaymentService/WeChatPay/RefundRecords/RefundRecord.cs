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

        #region Common

        [NotNull]
        public virtual string OutRefundNo { get; protected set; }

        [NotNull]
        public virtual string TransactionId { get; protected set; }

        [NotNull]
        public virtual string OutTradeNo { get; protected set; }

        [CanBeNull]
        public virtual string SuccessTime { get; protected set; }

        #endregion

        #region V2-only

        [NotNull]
        public virtual string ReturnCode { get; protected set; }

        [CanBeNull]
        public virtual string ReturnMsg { get; protected set; }

        [NotNull]
        public virtual string AppId { get; protected set; }

        [NotNull]
        public virtual string MchId { get; protected set; }

        [NotNull]
        public virtual string RefundId { get; protected set; }

        public virtual int TotalFee { get; protected set; }

        public virtual int? SettlementTotalFee { get; protected set; }

        public virtual int RefundFee { get; protected set; }

        public virtual int? SettlementRefundFee { get; protected set; }

        [CanBeNull]
        public virtual string FeeType { get; protected set; }

        public virtual int CashFee { get; protected set; }

        [CanBeNull]
        public virtual string CashFeeType { get; protected set; }

        public virtual int? CashRefundFee { get; protected set; }

        public virtual int? CouponRefundFee { get; protected set; }

        public virtual int? CouponRefundCount { get; protected set; }

        [CanBeNull]
        public virtual string CouponTypes { get; protected set; }

        [CanBeNull]
        public virtual string CouponIds { get; protected set; }

        [CanBeNull]
        public virtual string CouponRefundFees { get; protected set; }

        [NotNull]
        public virtual string RefundStatus { get; protected set; }

        /// <summary>
        /// It's really named "Accout" by stupid WeChat team!
        /// </summary>
        [NotNull]
        public virtual string RefundRecvAccout { get; protected set; }

        [NotNull]
        public virtual string RefundAccount { get; protected set; }

        [NotNull]
        public virtual string RefundRequestSource { get; protected set; }

        #endregion

        #region V3-only

        [NotNull]
        public virtual string Channel { get; protected set; }

        [NotNull]
        public virtual string UserReceivedAccount { get; protected set; }

        [NotNull]
        public virtual string CreateTime { get; protected set; }

        [NotNull]
        public virtual string Status { get; protected set; }

        [NotNull]
        public virtual string FundsAccount { get; protected set; }

        /// <summary>
        /// Serialized Amount object
        /// </summary>
        [NotNull]
        public virtual string Amount { get; protected set; }

        /// <summary>
        /// Serialized array[Promotion] object
        /// </summary>
        [NotNull]
        public virtual string PromotionDetail { get; protected set; }

        #endregion

        protected RefundRecord()
        {
        }

        /// <summary>
        /// V2 constructor.
        /// </summary>
        public RefundRecord(
            Guid id,
            Guid? tenantId,
            Guid paymentId,
            string returnCode,
            string returnMsg,
            string appId,
            string mchId,
            string transactionId,
            string outTradeNo,
            string refundId,
            string outRefundNo,
            int totalFee,
            int? settlementTotalFee,
            int refundFee,
            int? settlementRefundFee,
            string feeType,
            int cashFee,
            string cashFeeType,
            int? cashRefundFee,
            int? couponRefundFee,
            int? couponRefundCount,
            string couponTypes,
            string couponIds,
            string couponRefundFees
        ) : base(id)
        {
            TenantId = tenantId;
            PaymentId = paymentId;
            ReturnCode = returnCode;
            ReturnMsg = returnMsg;
            AppId = appId;
            MchId = mchId;
            TransactionId = transactionId;
            OutTradeNo = outTradeNo;
            RefundId = refundId;
            OutRefundNo = outRefundNo;
            TotalFee = totalFee;
            SettlementTotalFee = settlementTotalFee;
            RefundFee = refundFee;
            SettlementRefundFee = settlementRefundFee;
            FeeType = feeType;
            CashFee = cashFee;
            CashFeeType = cashFeeType;
            CashRefundFee = cashRefundFee;
            CouponRefundFee = couponRefundFee;
            CouponRefundCount = couponRefundCount;
            CouponTypes = couponTypes;
            CouponIds = couponIds;
            CouponRefundFees = couponRefundFees;
        }

        /// <summary>
        /// V3 constructor.
        /// </summary>
        public RefundRecord(
            Guid id,
            Guid? tenantId,
            Guid paymentId,
            string refundId,
            string outRefundNo,
            string transactionId,
            string outTradeNo,
            string channel,
            string userReceivedAccount,
            string successTime,
            string createTime,
            string status,
            string fundsAccount,
            string amount,
            string promotionDetail) : base(id)
        {
            TenantId = tenantId;
            PaymentId = paymentId;
            RefundId = refundId;
            OutRefundNo = outRefundNo;
            TransactionId = transactionId;
            OutTradeNo = outTradeNo;
            Channel = channel;
            UserReceivedAccount = userReceivedAccount;
            SuccessTime = successTime;
            CreateTime = createTime;
            Status = status;
            FundsAccount = fundsAccount;
            Amount = amount;
            PromotionDetail = promotionDetail;
        }

        public void SetInformationInNotifyV2(
            string refundStatus,
            string successTime,
            string refundRecvAccout,
            string refundAccount,
            string refundRequestSource,
            int settlementRefundFee)
        {
            RefundStatus = refundStatus;
            SuccessTime = successTime;
            RefundRecvAccout = refundRecvAccout;
            RefundAccount = refundAccount;
            RefundRequestSource = refundRequestSource;
            SettlementRefundFee = settlementRefundFee;
        }

        public void SetInformationInNotify(
            string status,
            string successTime,
            string userReceivedAccount,
            string amount)
        {
            Status = status; // 在通知报文中为refund_status……
            SuccessTime = successTime;
            UserReceivedAccount = userReceivedAccount;
            Amount = amount;
        }
    }
}