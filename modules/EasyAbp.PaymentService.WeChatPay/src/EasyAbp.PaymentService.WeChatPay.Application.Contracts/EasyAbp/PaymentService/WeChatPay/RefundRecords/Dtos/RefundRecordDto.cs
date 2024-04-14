using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.WeChatPay.RefundRecords.Dtos
{
    [Serializable]
    public class RefundRecordDto : ExtensibleCreationAuditedEntityDto<Guid>
    {
        public Guid PaymentId { get; set; }

        #region Common

        public string OutRefundNo { get; set; }

        public string TransactionId { get; set; }

        public string OutTradeNo { get; set; }

        public string SuccessTime { get; set; }

        #endregion

        #region V2-only

        public string ReturnCode { get; set; }

        public string ReturnMsg { get; set; }

        public string AppId { get; set; }

        public string MchId { get; set; }

        public string RefundId { get; set; }

        public int TotalFee { get; set; }

        public int? SettlementTotalFee { get; set; }

        public int RefundFee { get; set; }

        public int? SettlementRefundFee { get; set; }

        public string FeeType { get; set; }

        public int CashFee { get; set; }

        public string CashFeeType { get; set; }

        public int? CashRefundFee { get; set; }

        public int? CouponRefundFee { get; set; }

        public int? CouponRefundCount { get; set; }

        public string CouponTypes { get; set; }

        public string CouponIds { get; set; }

        public string CouponRefundFees { get; set; }

        public string RefundStatus { get; set; }

        /// <summary>
        /// It's really named "Accout" by stupid WeChat team!
        /// </summary>
        public string RefundRecvAccout { get; set; }

        public string RefundAccount { get; set; }

        public string RefundRequestSource { get; set; }

        #endregion

        #region V3-only

        public string Channel { get; set; }

        public string UserReceivedAccount { get; set; }

        public string CreateTime { get; set; }

        public string Status { get; set; }

        public string FundsAccount { get; set; }

        /// <summary>
        /// Serialized Amount object
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// Serialized array[Promotion] object
        /// </summary>
        public string PromotionDetail { get; set; }

        #endregion
    }
}