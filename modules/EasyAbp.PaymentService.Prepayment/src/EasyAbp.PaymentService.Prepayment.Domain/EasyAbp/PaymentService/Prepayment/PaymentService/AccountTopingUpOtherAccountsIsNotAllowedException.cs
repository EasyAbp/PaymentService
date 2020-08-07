using Volo.Abp;

namespace EasyAbp.PaymentService.Prepayment.PaymentService
{
    public class AccountTopingUpOtherAccountsIsNotAllowedException : BusinessException
    {
        public AccountTopingUpOtherAccountsIsNotAllowedException(string accountGroupName) : base(
            message: $"The account ({accountGroupName}) is not allowed to top up other accounts.")
        {
        }
    }
}