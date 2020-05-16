using System.Threading.Tasks;

namespace EasyAbp.PaymentService.Web.Pages.PaymentService.Payments.PaymentItem
{
    public class IndexModel : PaymentServicePageModel
    {
        public async Task OnGetAsync()
        {
            await Task.CompletedTask;
        }
    }
}
