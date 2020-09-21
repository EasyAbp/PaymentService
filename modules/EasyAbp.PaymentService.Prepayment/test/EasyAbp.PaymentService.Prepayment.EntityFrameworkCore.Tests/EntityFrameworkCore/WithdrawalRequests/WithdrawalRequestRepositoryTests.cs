using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.WithdrawalRequests;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.PaymentService.Prepayment.EntityFrameworkCore.WithdrawalRequests
{
    public class WithdrawalRequestRepositoryTests : PrepaymentEntityFrameworkCoreTestBase
    {
        private readonly IWithdrawalRequestRepository _withdrawalRequestRepository;

        public WithdrawalRequestRepositoryTests()
        {
            _withdrawalRequestRepository = GetRequiredService<IWithdrawalRequestRepository>();
        }

        /*
        [Fact]
        public async Task Test1()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                // Arrange

                // Act

                //Assert
            });
        }
        */
    }
}
