using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Prepayment.Accounts.Dtos
{
    [Serializable]
    public class AccountDto : FullAuditedEntityDto<Guid>
    {
        public string AccountGroupName { get; set; }

        public Guid UserId { get; set; }

        public decimal Balance { get; set; }

        public decimal LockedBalance { get; set; }
    }
}