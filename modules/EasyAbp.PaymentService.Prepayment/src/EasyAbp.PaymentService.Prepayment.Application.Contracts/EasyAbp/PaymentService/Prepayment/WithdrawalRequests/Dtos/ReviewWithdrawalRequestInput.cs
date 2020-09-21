using System;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRequests.Dtos
{
    [Serializable]
    public class ReviewWithdrawalRequestInput
    {
        public bool IsApproved { get; set; }
    }
}