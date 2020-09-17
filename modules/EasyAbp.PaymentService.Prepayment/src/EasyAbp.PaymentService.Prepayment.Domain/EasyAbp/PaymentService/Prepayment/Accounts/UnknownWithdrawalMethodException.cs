using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class UnknownWithdrawalMethodException : BusinessException
    {
        public UnknownWithdrawalMethodException(string withdrawalMethod) : base(
            message: $"Withdrawal method {withdrawalMethod} does not exist.")
        {
        }
    }
}