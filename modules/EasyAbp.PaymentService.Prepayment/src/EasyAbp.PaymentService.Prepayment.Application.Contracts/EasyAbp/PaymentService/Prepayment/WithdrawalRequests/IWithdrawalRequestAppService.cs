using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.WithdrawalRequests.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRequests
{
    public interface IWithdrawalRequestAppService :
        IReadOnlyAppService< 
            WithdrawalRequestDto, 
            Guid, 
            GetWithdrawalRequestListInput>
    {
        Task<WithdrawalRequestDto> ReviewAsync(Guid id, ReviewWithdrawalRequestInput input);
    }
}