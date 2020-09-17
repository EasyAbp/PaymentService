using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRecords
{
    public class WithdrawalRecord : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        
        public virtual Guid AccountId { get; protected set; }
        
        [NotNull]
        public virtual string WithdrawalMethod { get; protected set; }
        
        public virtual decimal Amount { get; protected set; }
        
        public virtual DateTime? CompletionTime { get; protected set; }
        
        public virtual DateTime? CancellationTime { get; protected set; }
        
        [CanBeNull]
        public virtual string ResultErrorCode { get; protected set; }
        
        [CanBeNull]
        public virtual string ResultErrorMessage { get; protected set; }

        protected WithdrawalRecord()
        {
        }

        public WithdrawalRecord(
            Guid id, 
            Guid? tenantId, 
            Guid accountId, 
            [NotNull] string withdrawalMethod, 
            decimal amount
        ) : base(id)
        {
            TenantId = tenantId;
            AccountId = accountId;
            WithdrawalMethod = withdrawalMethod;
            Amount = amount;
        }

        public void Complete(DateTime completionTime)
        {
            CheckWithdrawalIsInProgress();

            CompletionTime = completionTime;
        }
        
        public void Cancel(
            DateTime cancellationTime,
            [CanBeNull] string resultErrorCode,
            [CanBeNull] string resultErrorMessage)
        {
            CheckWithdrawalIsInProgress();
            
            CancellationTime = cancellationTime;
            ResultErrorCode = resultErrorCode;
            ResultErrorMessage = resultErrorMessage;
        }

        private void CheckWithdrawalIsInProgress()
        {
            if (CompletionTime.HasValue || CancellationTime.HasValue)
            {
                throw new WithdrawalIsNotInProgressException();
            }
        }
    }
}
