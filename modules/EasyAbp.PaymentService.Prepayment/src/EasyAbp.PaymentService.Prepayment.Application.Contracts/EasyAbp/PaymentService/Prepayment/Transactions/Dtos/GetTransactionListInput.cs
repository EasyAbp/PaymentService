using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Prepayment.Transactions.Dtos
{
    [Serializable]
    public class GetTransactionListInput : PagedAndSortedResultRequestDto
    {
        public Guid AccountId { get; set; }
    }
}