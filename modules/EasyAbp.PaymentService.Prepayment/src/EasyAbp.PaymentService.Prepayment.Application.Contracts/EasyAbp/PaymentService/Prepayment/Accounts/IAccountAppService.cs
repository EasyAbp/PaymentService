using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Accounts.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public interface IAccountAppService :
        IReadOnlyAppService< 
            AccountDto, 
            Guid, 
            GetAccountListInput>
    {
        Task<AccountDto> ChangeBalanceAsync(Guid id, ChangeBalanceInput input);
        
        Task<AccountDto> ChangeLockedBalanceAsync(Guid id, ChangeLockedBalanceInput input);
        
        Task TopUpAsync(Guid id, TopUpInput input);
    }
}