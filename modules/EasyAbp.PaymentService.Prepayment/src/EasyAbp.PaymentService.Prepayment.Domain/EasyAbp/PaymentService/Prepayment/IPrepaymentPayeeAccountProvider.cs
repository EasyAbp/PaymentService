using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Accounts;

namespace EasyAbp.PaymentService.Prepayment
{
    public interface IPrepaymentPayeeAccountProvider
    {
        Task<string> GetPayeeAccountAsync(Account account);

        Task<Account> GetAccountByPayeeAccountAsync(string payeeAccount);
    }
}