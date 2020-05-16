using System;
using EasyAbp.PaymentService.Payments.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentAppService :
        ICrudAppService< 
            PaymentDto, 
            Guid, 
            PagedAndSortedResultRequestDto,
            CreatePaymentDto,
            object>
    {

    }
}