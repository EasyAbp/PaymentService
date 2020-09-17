using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.WithdrawalRecords;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.PaymentService.Prepayment.EntityFrameworkCore.WithdrawalRecords
{
    public class WithdrawalRecordRepositoryTests : PrepaymentEntityFrameworkCoreTestBase
    {
        private readonly IWithdrawalRecordRepository _withdrawalRecordRepository;

        public WithdrawalRecordRepositoryTests()
        {
            _withdrawalRecordRepository = GetRequiredService<IWithdrawalRecordRepository>();
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
