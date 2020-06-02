using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.WeChatPay.PaymentRecords
{
    public class PaymentRecord : CreationAuditedAggregateRoot<Guid>, IMultiTenant
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

        [CanBeNull]
        public virtual string DeviceInfo { get; protected set; }
        
        [NotNull]
        public virtual string NonceStr { get; protected set; }

        [NotNull]
        public virtual string Sign { get; protected set; }

        [CanBeNull]
        public virtual string SignType { get; protected set; }

        [NotNull]
        public virtual string ResultCode { get; protected set; }

        [CanBeNull]
        public virtual string ErrCode { get; protected set; }

        [CanBeNull]
        public virtual string ErrCodeDes { get; protected set; }

        [NotNull]
        public virtual string Openid { get; protected set; }

        [NotNull]
        public virtual string IsSubscribe { get; protected set; }

        [NotNull]
        public virtual string TradeType { get; protected set; }

        [NotNull]
        public virtual string BankType { get; protected set; }

        public virtual int TotalFee { get; protected set; }
        
        public virtual int? SettlementTotalFee { get; protected set; }
        
        [CanBeNull]
        public virtual string FeeType { get; protected set; }
        
        public virtual int CashFee { get; protected set; }
        
        [CanBeNull]
        public virtual string CashFeeType { get; protected set; }
        
        public virtual int? CouponFee { get; protected set; }
        
        public virtual int? CouponCount { get; protected set; }
        
        [CanBeNull]
        public virtual string CouponTypes { get; protected set; }
        
        [CanBeNull]
        public virtual string CouponIds { get; protected set; }
        
        [CanBeNull]
        public virtual string CouponFees { get; protected set; }
        
        [NotNull]
        public virtual string TransactionId { get; protected set; }
        
        [NotNull]
        public virtual string OutTradeNo { get; protected set; }

        [CanBeNull]
        public virtual string Attach { get; protected set; }
        
        [NotNull]
        public virtual string TimeEnd { get; protected set; }

        protected PaymentRecord()
        {
        }

        public PaymentRecord(
            Guid id,
            Guid? tenantId,
            Guid paymentId) : base(id)
        {
            TenantId = tenantId;
            PaymentId = paymentId;
        }

        public void SetResult(
            string returnCode,
            string returnMsg,
            string appId,
            string mchId,
            string deviceInfo,
            string nonceStr,
            string sign,
            string signType,
            string resultCode,
            string errCode,
            string errCodeDes,
            string openid,
            string isSubscribe,
            string tradeType,
            string bankType,
            int totalFee,
            int? settlementTotalFee,
            string feeType,
            int cashFee,
            string cashFeeType,
            int? couponFee,
            int? couponCount,
            string couponTypes,
            string couponIds,
            string couponFees,
            string transactionId,
            string outTradeNo,
            string attach,
            string timeEnd)
        {
            ReturnCode = returnCode;
            ReturnMsg = returnMsg;
            AppId = appId;
            MchId = mchId;
            DeviceInfo = deviceInfo;
            NonceStr = nonceStr;
            Sign = sign;
            SignType = signType;
            ResultCode = resultCode;
            ErrCode = errCode;
            ErrCodeDes = errCodeDes;
            Openid = openid;
            IsSubscribe = isSubscribe;
            TradeType = tradeType;
            BankType = bankType;
            TotalFee = totalFee;
            SettlementTotalFee = settlementTotalFee;
            FeeType = feeType;
            CashFee = cashFee;
            CashFeeType = cashFeeType;
            CouponFee = couponFee;
            CouponCount = couponCount;
            CouponTypes = couponTypes;
            CouponIds = couponIds;
            CouponFees = couponFees;
            TransactionId = transactionId;
            OutTradeNo = outTradeNo;
            Attach = attach;
            TimeEnd = timeEnd;
        }
    }
}
