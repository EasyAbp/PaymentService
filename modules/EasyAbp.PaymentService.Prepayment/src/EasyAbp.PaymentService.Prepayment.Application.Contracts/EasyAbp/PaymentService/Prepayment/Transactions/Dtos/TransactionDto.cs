using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Prepayment.Transactions.Dtos
{
    [Serializable]
    public class TransactionDto : CreationAuditedEntityDto<Guid>
    {
        public Guid AccountId { get; set; }

        public Guid AccountUserId { get; set; }

        public Guid? PaymentId { get; set; }

        public TransactionType TransactionType { get; set; }

        public string ActionName { get; set; }

        public string PaymentMethod { get; set; }

        public string OppositePartAccount { get; set; }

        public string ExternalTradingCode { get; set; }

        public decimal ChangedBalance { get; set; }

        public decimal OriginalBalance { get; set; }
    }
}