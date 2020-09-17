using System;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.PaymentService.Prepayment.Accounts.Dtos
{
    [Serializable]
    public class WithdrawInput : ExtensibleObject
    {
        public string WithdrawalMethod { get; set; }
        
        public decimal Amount { get; set; }
    }
}