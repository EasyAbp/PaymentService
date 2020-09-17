using System;
using EasyAbp.PaymentService.Prepayment.WithdrawalRecords.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRecords
{
    public interface IWithdrawalRecordAppService :
        IReadOnlyAppService< 
            WithdrawalRecordDto, 
            Guid, 
            PagedAndSortedResultRequestDto>
    {

    }
}