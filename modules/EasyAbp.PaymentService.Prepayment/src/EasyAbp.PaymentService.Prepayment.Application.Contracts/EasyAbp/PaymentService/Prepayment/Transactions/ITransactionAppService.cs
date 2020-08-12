using System;
using EasyAbp.PaymentService.Prepayment.Transactions.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.Prepayment.Transactions
{
    public interface ITransactionAppService :
        IReadOnlyAppService< 
            TransactionDto, 
            Guid, 
            GetTransactionListInput>
    {

    }
}