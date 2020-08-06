using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.PaymentService
{
    public class AccountRechargingOtherAccountsIsNotAllowedException : BusinessException
    {
        public AccountRechargingOtherAccountsIsNotAllowedException(string accountGroupName) : base(
            message: $"The account ({accountGroupName}) is not allowed to recharge other accounts.")
        {
        }
    }
}