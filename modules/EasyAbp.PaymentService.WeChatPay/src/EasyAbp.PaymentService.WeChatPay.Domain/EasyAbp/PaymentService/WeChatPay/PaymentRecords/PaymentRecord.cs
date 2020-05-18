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
    }
}