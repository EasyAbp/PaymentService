using System;
using EasyAbp.PaymentService.WeChatPay.RefundRecords.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.WeChatPay.RefundRecords
{
    public interface IRefundRecordAppService :
        ICrudAppService< 
            RefundRecordDto, 
            Guid, 
            PagedAndSortedResultRequestDto,
            object,
            object>
    {

    }
}