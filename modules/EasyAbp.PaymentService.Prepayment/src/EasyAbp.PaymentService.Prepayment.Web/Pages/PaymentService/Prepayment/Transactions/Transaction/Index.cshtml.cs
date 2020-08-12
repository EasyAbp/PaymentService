using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Users;

namespace EasyAbp.PaymentService.Prepayment.Web.Pages.PaymentService.Prepayment.Transactions.Transaction
{
    public class IndexModel : PrepaymentPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid AccountId { get; set; }
        
        public string UserName { get; set; }
        
        public string AccountGroupName { get; set; }

        public virtual async Task OnGetAsync()
        {
            var accountAppService = ServiceProvider.GetRequiredService<IAccountAppService>();
            var userLookupServiceProvider = ServiceProvider.GetRequiredService<IExternalUserLookupServiceProvider>();

            var account = await accountAppService.GetAsync(AccountId);

            var userData = await userLookupServiceProvider.FindByIdAsync(account.UserId);

            AccountGroupName = account.AccountGroupName;
            UserName = userData.UserName;
        }
    }
}
