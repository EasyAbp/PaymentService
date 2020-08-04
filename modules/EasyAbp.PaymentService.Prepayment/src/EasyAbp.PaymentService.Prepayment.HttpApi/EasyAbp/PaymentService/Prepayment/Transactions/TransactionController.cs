using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Transactions.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.PaymentService.Prepayment.Transactions
{
    [RemoteService(Name = "TransactionService")]
    [Route("/api/prepayment/transaction")]
    public class TransactionController : PrepaymentController, ITransactionAppService
    {
        private readonly ITransactionAppService _service;

        public TransactionController(ITransactionAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public Task<TransactionDto> GetAsync(Guid id)
        {
            return _service.GetAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<TransactionDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _service.GetListAsync(input);
        }
    }
}