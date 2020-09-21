using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRequests
{
    public class UnexpectedWithdrawalAmountException : BusinessException
    {
        public UnexpectedWithdrawalAmountException() : base("WrongWithdrawalAmount", "The refund amount is unexpected.")
        {
        }
    }
}