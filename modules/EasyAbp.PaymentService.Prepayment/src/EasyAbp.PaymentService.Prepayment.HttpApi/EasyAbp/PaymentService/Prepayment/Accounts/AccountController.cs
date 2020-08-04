using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Accounts.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    [RemoteService(Name = "AccountService")]
    [Route("/api/prepayment/account")]
    public class AccountController : PrepaymentController, IAccountAppService
    {
        private readonly IAccountAppService _service;

        public AccountController(IAccountAppService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("changeBalance")]
        public Task<AccountDto> ChangeBalanceAsync(ChangeBalanceInput input)
        {
            return _service.ChangeBalanceAsync(input);
        }

        [HttpPost]
        [Route("changeLockedBalance")]
        public Task<AccountDto> ChangeLockedBalanceAsync(ChangeLockedBalanceInput input)
        {
            return _service.ChangeLockedBalanceAsync(input);
        }

        [HttpPost]
        [Route("recharge")]
        public Task RechargeAsync(RechargeInput input)
        {
            return _service.RechargeAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<AccountDto> GetAsync(Guid id)
        {
            return _service.GetAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<AccountDto>> GetListAsync(GetAccountListInput input)
        {
            return _service.GetListAsync(input);
        }
    }
}