using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Accounts;
using EasyAbp.PaymentService.Prepayment.Accounts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EasyAbp.PaymentService.Prepayment.Web.Pages.PaymentService.Prepayment.Accounts.Account
{
    public class ChangeLockedBalanceModalModel : PrepaymentPageModel
    {
        private readonly IAccountAppService _service;

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public ChangeLockedBalanceInput ChangeLockedBalanceInput { get; set; }

        public AccountDto Account { get; set; }

        public ChangeLockedBalanceModalModel(IAccountAppService service)
        {
            _service = service;
        }

        public async Task OnGetAsync()
        {
            Account = await _service.GetAsync(Id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _service.ChangeLockedBalanceAsync(Id, ChangeLockedBalanceInput);
            return NoContent();
        }
    }
}