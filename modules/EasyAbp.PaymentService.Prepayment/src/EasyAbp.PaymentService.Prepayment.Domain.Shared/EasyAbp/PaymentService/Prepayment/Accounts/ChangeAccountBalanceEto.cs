using System;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class ChangeAccountBalanceEto : IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid AccountId { get; set; }
        
        public decimal ChangedBalance { get; set; }
    }
}