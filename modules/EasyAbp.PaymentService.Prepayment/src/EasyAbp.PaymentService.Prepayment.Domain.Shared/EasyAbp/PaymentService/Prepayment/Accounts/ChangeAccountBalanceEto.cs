﻿using System;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class ChangeAccountBalanceEto : ExtensibleObject, IHasExtraProperties, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid AccountId { get; set; }
        
        public decimal ChangedBalance { get; set; }
        
        /// <summary>
        /// Default value is PrepaymentConsts.ChangeBalanceActionName.
        /// </summary>
        public string ActionName { get; set; }
        
        /// <summary>
        /// The value will be copied to Transaction.ExtraProperties.
        /// </summary>
        // public ExtraPropertyDictionary ExtraProperties { get; set; }

        public ChangeAccountBalanceEto(
            Guid? tenantId,
            Guid accountId,
            decimal changedBalance,
            string actionName = PrepaymentConsts.ChangeBalanceActionName)
        {
            TenantId = tenantId;
            AccountId = accountId;
            ChangedBalance = changedBalance;
            ActionName = actionName;

            ExtraProperties = new ExtraPropertyDictionary();
        }
    }
}