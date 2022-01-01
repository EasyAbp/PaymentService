using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Payments
{
    [RemoteService(Name = PaymentServiceRemoteServiceConsts.RemoteServiceName)]
    [Route("/api/payment-service/payment")]
    public class PaymentController : PaymentServiceController, IPaymentAppService
    {
        private readonly IPaymentAppService _service;

        public PaymentController(IPaymentAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<PaymentDto> GetAsync(Guid id)
        {
            return _service.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<PaymentDto>> GetListAsync(GetPaymentListInput input)
        {
            return _service.GetListAsync(input);
        }

        [HttpGet]
        [Route("payment-method")]
        public virtual Task<ListResultDto<PaymentMethodDto>> GetListPaymentMethod()
        {
            return _service.GetListPaymentMethod();
        }

        [HttpPost]
        [Route("{id}/pay")]
        public virtual Task<PaymentDto> PayAsync(Guid id, PayInput input)
        {
            return _service.PayAsync(id, input);
        }

        [HttpPost]
        [Route("{id}/cancel")]
        public virtual Task<PaymentDto> CancelAsync(Guid id)
        {
            return _service.CancelAsync(id);
        }

        [HttpPost]
        [Route("{id}/refund/rollback")]
        public virtual Task<PaymentDto> RefundRollbackAsync(Guid id)
        {
            return _service.RefundRollbackAsync(id);
        }
    }
}