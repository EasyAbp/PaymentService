using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Users;

namespace EasyAbp.PaymentService.Prepayment.Web.Pages.PaymentService.Prepayment.Accounts.Account
{
    public class IndexModel : PrepaymentPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid? UserId { get; set; }
        
        public string UserName { get; set; }
        
        public virtual async Task OnGetAsync()
        {
            if (!UserId.HasValue)
            {
                return;
            }
            
            var userLookupServiceProvider = ServiceProvider.GetRequiredService<IExternalUserLookupServiceProvider>();

            var userData = await userLookupServiceProvider.FindByIdAsync(UserId.Value);

            UserName = userData.UserName;
        }
    }
}
