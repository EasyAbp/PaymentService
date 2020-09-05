using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Prepayment.Accounts.Dtos
{
    [Serializable]
    public class GetAccountListInput : PagedAndSortedResultRequestDto
    {
        public Guid? UserId { get; set; }
    }
}