using System;
using System.Collections.Generic;
using System.Linq;
using EasyAbp.PaymentService.Refunds;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Payments
{
    public class Payment : FullAuditedAggregateRoot<Guid>, IPayment, IMultiTenant
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

        public virtual DateTime? CanceledTime { get; protected set; }

        IEnumerable<IPaymentItem> IPayment.PaymentItems => PaymentItems;
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
            OriginalPaymentAmount = originalPaymentAmount.EnsureIsNonNegative();
            ActualPaymentAmount = originalPaymentAmount.EnsureIsNonNegative();
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

        public void SetPaymentDiscount(PaymentItem paymentItem, decimal paymentDiscount)
        {
            CheckIsInProgress();

            paymentItem.SetPaymentDiscount(paymentDiscount);

            PaymentDiscount = PaymentItems.Sum(item => item.PaymentDiscount).EnsureIsNonNegative();
            ActualPaymentAmount = (OriginalPaymentAmount - paymentDiscount).EnsureIsNonNegative();
        }

        public void CompletePayment(DateTime completionTime)
        {
            CheckIsInProgress();

            CompletionTime = completionTime;
        }

        public void CancelPayment(DateTime canceledTime)
        {
            CheckIsInProgress();

            CanceledTime = canceledTime;
        }

        public void StartRefund(Refund refund)
        {
            if (IsCanceled() || !IsCompleted())
            {
                throw new PaymentIsInUnexpectedStageException(Id);
            }

            if (PendingRefundAmount != decimal.Zero)
            {
                throw new AnotherRefundTaskIsOnGoingException(Id);
            }

            var refundAmount = refund.RefundAmount.EnsureIsNonNegative();

            if (ActualPaymentAmount < RefundAmount + refundAmount || refund.RefundItems.Any(item =>
                    !PaymentItems.First(x => x.Id == item.PaymentItemId).TryStartRefund(item.RefundAmount)))
            {
                throw new InvalidRefundAmountException(Id, refundAmount);
            }

            PendingRefundAmount = refundAmount;
        }

        public void CompleteRefund()
        {
            if (IsCanceled() || !IsCompleted() || PendingRefundAmount.EnsureIsNonNegative() <= decimal.Zero)
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

        public void RollbackRefund()
        {
            if (IsCanceled() || !IsCompleted())
            {
                throw new PaymentIsInUnexpectedStageException(Id);
            }

            foreach (var paymentItem in PaymentItems)
            {
                paymentItem.TryRollbackRefund();
            }

            PendingRefundAmount = decimal.Zero;
        }

        public bool IsCanceled()
        {
            return CanceledTime.HasValue;
        }

        public bool IsCompleted()
        {
            return CompletionTime.HasValue;
        }

        public bool IsInProgress()
        {
            return !IsCanceled() && !IsCompleted();
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