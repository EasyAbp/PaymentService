using System;
using EasyAbp.PaymentService.Refunds.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.Refunds
{
    public interface IRefundAppService :
        IReadOnlyAppService< 
            RefundDto, 
            Guid, 
            GetRefundListInput>
    {

    }
}