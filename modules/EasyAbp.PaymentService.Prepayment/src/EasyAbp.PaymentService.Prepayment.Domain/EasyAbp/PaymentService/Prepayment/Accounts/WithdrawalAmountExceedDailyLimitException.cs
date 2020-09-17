using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class WithdrawalAmountExceedDailyLimitException : BusinessException
    {
        public WithdrawalAmountExceedDailyLimitException() : base(
            "WithdrawalAmountExceedDailyLimit",
            "The maximum daily withdrawal limit has been exceeded.")
        {
        }
    }
}