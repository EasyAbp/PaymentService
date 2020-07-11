using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.WeChatPay.PaymentRecords
{
    [RemoteService(Name = "PaymentServiceWeChatPay")]
    [Route("/api/paymentService/weChatPay/paymentRecord")]
    public class PaymentRecordController : WeChatPayController, IPaymentRecordAppService
    {
        private readonly IPaymentRecordAppService _service;

        public PaymentRecordController(IPaymentRecordAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public Task<PaymentRecordDto> GetAsync(Guid id)
        {
            return _service.GetAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<PaymentRecordDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _service.GetListAsync(input);
        }
    }
}