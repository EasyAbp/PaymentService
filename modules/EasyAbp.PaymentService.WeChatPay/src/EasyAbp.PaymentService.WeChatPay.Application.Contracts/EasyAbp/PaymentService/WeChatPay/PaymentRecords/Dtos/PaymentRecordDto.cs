using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.WeChatPay.PaymentRecords.Dtos
{
    public class PaymentRecordDto : CreationAuditedEntityDto<Guid>
    {
        public Guid PaymentId { get; set; }

        public string ReturnCode { get; set; }

        public string ReturnMsg { get; set; }

        public string AppId { get; set; }

        public string MchId { get; set; }

        public string DeviceInfo { get; set; }

        public string ResultCode { get; set; }

        public string ErrCode { get; set; }

        public string ErrCodeDes { get; set; }

        public string Openid { get; set; }

        public string IsSubscribe { get; set; }

        public string TradeType { get; set; }

        public string BankType { get; set; }

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

        public string TransactionId { get; set; }

        public string OutTradeNo { get; set; }

        public string Attach { get; set; }

        public string TimeEnd { get; set; }
    }
}