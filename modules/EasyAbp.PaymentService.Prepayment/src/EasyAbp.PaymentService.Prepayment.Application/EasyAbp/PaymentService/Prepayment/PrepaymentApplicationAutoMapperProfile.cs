using EasyAbp.PaymentService.Prepayment.Accounts;
using EasyAbp.PaymentService.Prepayment.Accounts.Dtos;
using EasyAbp.PaymentService.Prepayment.Transactions;
using EasyAbp.PaymentService.Prepayment.Transactions.Dtos;
using AutoMapper;

namespace EasyAbp.PaymentService.Prepayment
{
    public class PrepaymentApplicationAutoMapperProfile : Profile
    {
        public PrepaymentApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<Account, AccountDto>();
            CreateMap<Transaction, TransactionDto>();
        }
    }
}
