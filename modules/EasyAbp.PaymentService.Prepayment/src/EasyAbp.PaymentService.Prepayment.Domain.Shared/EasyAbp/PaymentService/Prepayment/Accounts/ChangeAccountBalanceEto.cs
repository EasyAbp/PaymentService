using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    [Serializable]
    public class ChangeAccountBalanceEto : IMultiTenant, IHasExtraProperties
    {
        public Guid? TenantId { get; set; }

        public Guid AccountId { get; set; }
        
        public decimal ChangedBalance { get; set; }

        /// <summary>
        /// Default value is PrepaymentConsts.ChangeBalanceActionName.
        /// </summary>
        public string ActionName { get; set; } = PrepaymentConsts.ChangeBalanceActionName;

        /// <summary>
        /// The value will be copied to Transaction.ExtraProperties.
        /// </summary>
        public Dictionary<string, object> ExtraProperties { get; set; } = new Dictionary<string, object>();
    }
}