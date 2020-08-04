using System.Threading.Tasks;

namespace EasyAbp.PaymentService.Prepayment.Web.Pages.PaymentService.Prepayment.Transactions.Transaction
{
    public class IndexModel : PrepaymentPageModel
    {
        public virtual async Task OnGetAsync()
        {
            await Task.CompletedTask;
        }
    }
}
