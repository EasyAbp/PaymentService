using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.WeChatPay.PaymentRecords
{
    [RemoteService(Name = PaymentServiceRemoteServiceConsts.RemoteServiceName)]
    [Route("/api/payment-service/wechat-pay/payment-record")]
    public class PaymentRecordController : WeChatPayController, IPaymentRecordAppService
    {
        private readonly IPaymentRecordAppService _service;

        public PaymentRecordController(IPaymentRecordAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<PaymentRecordDto> GetAsync(Guid id)
        {
            return _service.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<PaymentRecordDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _service.GetListAsync(input);
        }
    }
}