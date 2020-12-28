using System;
using EasyAbp.PaymentService.Prepayment.WithdrawalRecords.Dtos;
using Volo.Abp.Application.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRecords
{
    [RemoteService(Name = "EasyAbpPaymentServicePrepayment")]
    [Route("/api/payment-service/prepayment/withdrawal-record")]
    public class WithdrawalRecordController : PrepaymentController, IWithdrawalRecordAppService
    {
        private readonly IWithdrawalRecordAppService _service;

        public WithdrawalRecordController(IWithdrawalRecordAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public Task<WithdrawalRecordDto> GetAsync(Guid id)
        {
            return _service.GetAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<WithdrawalRecordDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _service.GetListAsync(input);

        }
    }
}