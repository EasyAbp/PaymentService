using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRequests
{
    public class WithdrawalRequestHasBeenReviewedException : BusinessException
    {
        public WithdrawalRequestHasBeenReviewedException() : base("WithdrawalRequestHasBeenReviewed",
            "The review has been reviewed.")
        {
        }
    }
}