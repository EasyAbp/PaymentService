using System.Threading.Tasks;

namespace EasyAbp.PaymentService.Prepayment.Web.Pages.PaymentService.Prepayment.WithdrawalRequests.WithdrawalRequest
{
    public class IndexModel : PrepaymentPageModel
    {
        public virtual async Task OnGetAsync()
        {
            await Task.CompletedTask;
        }
    }
}
