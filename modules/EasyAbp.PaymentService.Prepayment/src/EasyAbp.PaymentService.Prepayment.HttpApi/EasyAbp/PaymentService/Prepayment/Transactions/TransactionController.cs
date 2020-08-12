using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Transactions.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Prepayment.Transactions
{
    [RemoteService(Name = "EasyAbpPaymentServicePrepayment")]
    [Route("/api/paymentService/prepayment/transaction")]
    public class TransactionController : PrepaymentController, ITransactionAppService
    {
        private readonly ITransactionAppService _service;

        public TransactionController(ITransactionAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<TransactionDto> GetAsync(Guid id)
        {
            return _service.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<TransactionDto>> GetListAsync(GetTransactionListInput input)
        {
            return _service.GetListAsync(input);
        }
    }
}