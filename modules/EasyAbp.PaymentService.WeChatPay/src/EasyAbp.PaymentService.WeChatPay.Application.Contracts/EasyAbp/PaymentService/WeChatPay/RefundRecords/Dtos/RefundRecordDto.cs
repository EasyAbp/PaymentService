using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.WeChatPay.RefundRecords.Dtos
{
    public class RefundRecordDto : CreationAuditedEntityDto<Guid>
    {
        public Guid PaymentId { get; set; }

        public string ReturnCode { get; set; }

        public string ReturnMsg { get; set; }

        public string AppId { get; set; }

        public string MchId { get; set; }

        public string NonceStr { get; set; }

        public string ReqInfo { get; set; }

        public string TransactionId { get; set; }

        public string OutTradeNo { get; set; }

        public string RefundId { get; set; }

        public string OutRefundNo { get; set; }

        public int TotalFee { get; set; }

        public int? SettlementTotalFee { get; set; }

        public int RefundFee { get; set; }

        public int SettlementRefundFee { get; set; }

        public string RefundStatus { get; set; }

        public string SuccessTime { get; set; }

        public string RefundRecvAccout { get; set; }

        public string RefundAccount { get; set; }

        public string RefundRequestSource { get; set; }
    }
}