using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRecords.Dtos
{
    [Serializable]
    public class WithdrawalRecordDto : FullAuditedEntityDto<Guid>
    {
        public Guid AccountId { get; set; }

        public string WithdrawalMethod { get; set; }

        public decimal Amount { get; set; }

        public DateTime? CompletionTime { get; set; }

        public DateTime? CancellationTime { get; set; }

        public string ResultErrorCode { get; set; }

        public string ResultErrorMessage { get; set; }
    }
}