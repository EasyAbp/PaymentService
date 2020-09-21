using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRequests.Dtos
{
    public class GetWithdrawalRequestListInput : PagedAndSortedResultRequestDto
    {
        public bool PendingOnly { get; set; }
        
        public Guid? AccountId { get; set; }
        
        public Guid? AccountUserId { get; set; }
    }
}