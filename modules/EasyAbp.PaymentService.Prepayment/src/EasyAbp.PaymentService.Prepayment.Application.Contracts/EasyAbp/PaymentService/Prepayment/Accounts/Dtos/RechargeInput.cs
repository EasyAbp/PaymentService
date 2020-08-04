using System;

namespace EasyAbp.PaymentService.Prepayment.Accounts.Dtos
{
    [Serializable]
    public class RechargeInput
    {
        public string PaymentMethod { get; set; }

        public Guid AccountId { get; set; }
        
        public decimal Amount { get; set; }
    }
}