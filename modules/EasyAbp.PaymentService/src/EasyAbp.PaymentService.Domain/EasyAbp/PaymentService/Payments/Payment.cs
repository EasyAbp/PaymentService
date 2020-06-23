using System;
using System.Collections.Generic;
using System.Linq;
using EasyAbp.PaymentService.Refunds;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Payments
{
    public class Payment : FullAuditedAggregateRoot<Guid>, IPaymentEntity
    {
        public virtual Guid? TenantId { get; protected set; }
        
        public virtual Guid UserId { get; protected set; }
        
        [NotNull]
        public virtual string PaymentMethod { get; protected set; }
        
        [CanBeNull]
        public virtual string PayeeAccount { get; protected set; }
        
        [CanBeNull]
        public virtual string ExternalTradingCode { get; protected set; }
        
        [NotNull]
        public virtual string Currency { get; protected set; }
        
        public virtual decimal OriginalPaymentAmount { get; protected set; }

        public virtual decimal PaymentDiscount { get; protected set; }
        
        public virtual decimal ActualPaymentAmount { get; protected set; }
        
        public virtual decimal RefundAmount { get; protected set; }
        
        public virtual decimal PendingRefundAmount { get; protected set; }
        
        public virtual DateTime? CompletionTime { get; protected set; }
        
        public virtual DateTime? CancelledTime { get; protected set; }
        
        public virtual List<PaymentItem> PaymentItems { get; protected set; }

        protected Payment()
        {
        }

        public Payment(
            Guid id,
            Guid? tenantId,
            Guid userId,
            [NotNull] string paymentMethod,
            [NotNull] string currency,
            decimal originalPaymentAmount,
            List<PaymentItem> paymentItems
        ) : base(id)
        {
            TenantId = tenantId;
            UserId = userId;
            PaymentMethod = paymentMethod;
            Currency = currency;
            OriginalPaymentAmount = originalPaymentAmount;
            ActualPaymentAmount = originalPaymentAmount;
            PaymentItems = paymentItems;
            RefundAmount = decimal.Zero;
        }

        public void SetPayeeAccount([NotNull] string payeeAccount)
        {
            PayeeAccount = payeeAccount;
        }

        public void SetExternalTradingCode([NotNull] string externalTradingCode)
        {
            CheckIsInProgress();

            ExternalTradingCode = externalTradingCode;
        }

        public void SetPaymentDiscount(decimal paymentDiscount)
        {
            CheckIsInProgress();

            PaymentDiscount = paymentDiscount;
            ActualPaymentAmount -= paymentDiscount;
        }

        public void CompletePayment(DateTime completionTime)
        {
            CheckIsInProgress();

            CompletionTime = completionTime;
        }
        
        public void CancelPayment(DateTime cancelledTime)
        {
            CheckIsInProgress();

            CancelledTime = cancelledTime;
        }
        
        public void StartRefund(IEnumerable<RefundInfoModel> refundInfoModels)
        {
            if (IsCancelled() || !IsCompleted())
            {
                throw new PaymentIsInUnexpectedStageException(Id);
            }

            if (PendingRefundAmount != decimal.Zero)
            {
                throw new AnotherRefundTaskIsOnGoingException(Id);
            }

            var infoModels = refundInfoModels.ToList();
            
            var exceptItems = infoModels.Select(model => model.PaymentItem).Except(PaymentItems).ToList();
            
            if (exceptItems.Any())
            {
                throw new EntityNotFoundException(typeof(PaymentItem), new []{exceptItems.Select(x => x.Id)});
            }

            var refundAmount = infoModels.Sum(model => model.RefundAmount);

            if (refundAmount <= decimal.Zero || ActualPaymentAmount < RefundAmount + refundAmount ||
                infoModels.Any(model => !model.PaymentItem.TryStartRefund(model.RefundAmount)))
            {
                throw new InvalidRefundAmountException(Id, refundAmount);
            }

            PendingRefundAmount = refundAmount;
        }
        
        public void CompleteOngoingRefund()
        {
            if (IsCancelled() || !IsCompleted() || PendingRefundAmount <= decimal.Zero)
            {
                throw new PaymentIsInUnexpectedStageException(Id);
            }
            
            foreach (var paymentItem in PaymentItems)
            {
                paymentItem.TryCompleteRefund();
            }

            RefundAmount += PendingRefundAmount;

            PendingRefundAmount = decimal.Zero;
        }
        
        public void RollbackOngoingRefund()
        {
            if (IsCancelled() || !IsCompleted())
            {
                throw new PaymentIsInUnexpectedStageException(Id);
            }
            
            foreach (var paymentItem in PaymentItems)
            {
                paymentItem.TryRollbackRefund();
            }

            PendingRefundAmount = decimal.Zero;
        }

        public bool IsCancelled()
        {
            return CancelledTime.HasValue;
        }
        
        public bool IsCompleted()
        {
            return CompletionTime.HasValue;
        }
        
        public bool IsInProgress()
        {
            return !IsCancelled() && !IsCompleted();
        }

        private void CheckIsInProgress()
        {
            if (!IsInProgress())
            {
                throw new PaymentIsInUnexpectedStageException(Id);
            }
        }
    }
}
