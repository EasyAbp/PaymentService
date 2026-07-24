using EasyAbp.PaymentService.Prepayment.Accounts;
using EasyAbp.PaymentService.Prepayment.Accounts.Dtos;
using EasyAbp.PaymentService.Prepayment.Transactions;
using EasyAbp.PaymentService.Prepayment.Transactions.Dtos;
using EasyAbp.PaymentService.Prepayment.WithdrawalRecords;
using EasyAbp.PaymentService.Prepayment.WithdrawalRecords.Dtos;
using EasyAbp.PaymentService.Prepayment.WithdrawalRequests;
using EasyAbp.PaymentService.Prepayment.WithdrawalRequests.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace EasyAbp.PaymentService.Prepayment
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AccountToAccountDtoMapper : MapperBase<Account, AccountDto>
    {
        public override partial AccountDto Map(Account source);
        public override partial void Map(Account source, AccountDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TransactionToTransactionDtoMapper : MapperBase<Transaction, TransactionDto>
    {
        public override partial TransactionDto Map(Transaction source);
        public override partial void Map(Transaction source, TransactionDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class WithdrawalRecordToWithdrawalRecordDtoMapper : MapperBase<WithdrawalRecord, WithdrawalRecordDto>
    {
        public override partial WithdrawalRecordDto Map(WithdrawalRecord source);
        public override partial void Map(WithdrawalRecord source, WithdrawalRecordDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class WithdrawalRequestToWithdrawalRequestDtoMapper : MapperBase<WithdrawalRequest, WithdrawalRequestDto>
    {
        public override partial WithdrawalRequestDto Map(WithdrawalRequest source);
        public override partial void Map(WithdrawalRequest source, WithdrawalRequestDto destination);
    }
}
