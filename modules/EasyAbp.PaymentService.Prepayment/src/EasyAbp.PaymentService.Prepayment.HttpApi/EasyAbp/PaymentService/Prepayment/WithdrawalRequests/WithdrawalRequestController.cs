using System;
using EasyAbp.PaymentService.Prepayment.WithdrawalRequests.Dtos;
using Volo.Abp.Application.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRequests
{
    [RemoteService(Name = "EasyAbpPaymentServicePrepayment")]
    [Route("/api/paymentService/prepayment/withdrawalRequest")]
    public class WithdrawalRequestController : PrepaymentController, IWithdrawalRequestAppService
    {
        private readonly IWithdrawalRequestAppService _service;

        public WithdrawalRequestController(IWithdrawalRequestAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<WithdrawalRequestDto> GetAsync(Guid id)
        {
            return _service.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<WithdrawalRequestDto>> GetListAsync(GetWithdrawalRequestListInput input)
        {
            return _service.GetListAsync(input);
        }

        [HttpPost]
        [Route("{id}/review")]
        public virtual Task<WithdrawalRequestDto> ReviewAsync(Guid id, ReviewWithdrawalRequestInput input)
        {
            return _service.ReviewAsync(id, input);
        }
    }
}