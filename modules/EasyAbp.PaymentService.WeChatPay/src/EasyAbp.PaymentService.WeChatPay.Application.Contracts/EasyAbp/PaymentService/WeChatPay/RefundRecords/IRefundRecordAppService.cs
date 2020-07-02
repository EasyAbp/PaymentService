using System;
using EasyAbp.PaymentService.WeChatPay.RefundRecords.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.WeChatPay.RefundRecords
{
    public interface IRefundRecordAppService :
        IReadOnlyAppService< 
            RefundRecordDto, 
            Guid, 
            PagedAndSortedResultRequestDto>
    {

    }
}