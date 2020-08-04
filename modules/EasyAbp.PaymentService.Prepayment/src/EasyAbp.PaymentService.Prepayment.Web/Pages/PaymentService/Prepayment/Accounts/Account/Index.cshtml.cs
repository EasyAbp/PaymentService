using System.Threading.Tasks;

namespace EasyAbp.PaymentService.Prepayment.Web.Pages.PaymentService.Prepayment.Accounts.Account
{
    public class IndexModel : PrepaymentPageModel
    {
        public virtual async Task OnGetAsync()
        {
            await Task.CompletedTask;
        }
    }
}
