using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.WeChatPay.PaymentRecords.Dtos
{
    [Serializable]
    public class PaymentRecordDto : ExtensibleCreationAuditedEntityDto<Guid>
    {
        public Guid PaymentId { get; set; }

        #region Common

        public string AppId { get; set; }

        public string MchId { get; set; }

        public string OutTradeNo { get; set; }

        public string TransactionId { get; set; }

        public string TradeType { get; set; }

        public string BankType { get; set; }

        public string Attach { get; set; }

        #endregion

        #region V2-only

        public string ReturnCode { get; set; }

        public string ReturnMsg { get; set; }

        public string DeviceInfo { get; set; }

        public string ResultCode { get; set; }

        public string ErrCode { get; set; }

        public string ErrCodeDes { get; set; }

        public string Openid { get; set; }

        public string IsSubscribe { get; set; }

        public int TotalFee { get; set; }

        public int? SettlementTotalFee { get; set; }

        public string FeeType { get; set; }

        public int CashFee { get; set; }

        public string CashFeeType { get; set; }

        public int? CouponFee { get; set; }

        public int? CouponCount { get; set; }

        public string CouponTypes { get; set; }

        public string CouponIds { get; set; }

        public string CouponFees { get; set; }

        public string TimeEnd { get; set; }

        #endregion

        #region V3-only

        public string TradeState { get; set; }

        public string TradeStateDesc { get; set; }

        public string SuccessTime { get; set; }

        /// <summary>
        /// Serialized Payer object
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// Serialized Amount object
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// Serialized SceneInfo object
        /// </summary>
        public string SceneInfo { get; set; }

        /// <summary>
        /// Serialized PromotionDetail object
        /// </summary>
        public string PromotionDetail { get; set; }

        #endregion
    }
}