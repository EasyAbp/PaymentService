using System;
using EasyAbp.PaymentService.Prepayment.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRequests
{
    public class WithdrawalRequestRepository : EfCoreRepository<IPaymentServicePrepaymentDbContext, WithdrawalRequest, Guid>, IWithdrawalRequestRepository
    {
        public WithdrawalRequestRepository(IDbContextProvider<IPaymentServicePrepaymentDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}