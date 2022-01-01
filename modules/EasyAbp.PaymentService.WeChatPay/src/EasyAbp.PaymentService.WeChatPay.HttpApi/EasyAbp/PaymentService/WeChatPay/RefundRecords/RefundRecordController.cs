using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.WeChatPay.RefundRecords.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.WeChatPay.RefundRecords
{
    [RemoteService(Name = PaymentServiceRemoteServiceConsts.RemoteServiceName)]
    [Route("/api/payment-service/wechat-pay/refund-record")]
    public class RefundRecordController : WeChatPayController, IRefundRecordAppService
    {
        private readonly IRefundRecordAppService _service;

        public RefundRecordController(IRefundRecordAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<RefundRecordDto> GetAsync(Guid id)
        {
            return _service.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<RefundRecordDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _service.GetListAsync(input);
        }
    }
}