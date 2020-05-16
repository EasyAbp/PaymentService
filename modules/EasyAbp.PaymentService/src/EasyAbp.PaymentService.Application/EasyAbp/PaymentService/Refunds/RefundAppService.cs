using System;
using EasyAbp.PaymentService.Refunds.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.Refunds
{
    public class RefundAppService : CrudAppService<Refund, RefundDto, Guid, PagedAndSortedResultRequestDto, CreateRefundDto, object>,
        IRefundAppService
    {
        private readonly IRefundRepository _repository;

        public RefundAppService(IRefundRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}