using System;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.WeChatPay.PaymentRecords
{
    public interface IPaymentRecordAppService :
        IReadOnlyAppService< 
            PaymentRecordDto, 
            Guid, 
            PagedAndSortedResultRequestDto>
    {

    }
}