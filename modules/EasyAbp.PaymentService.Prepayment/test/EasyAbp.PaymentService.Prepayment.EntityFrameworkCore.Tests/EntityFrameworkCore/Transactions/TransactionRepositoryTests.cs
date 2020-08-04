using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Transactions;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.PaymentService.Prepayment.EntityFrameworkCore.Transactions
{
    public class TransactionRepositoryTests : PrepaymentEntityFrameworkCoreTestBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionRepositoryTests()
        {
            _transactionRepository = GetRequiredService<ITransactionRepository>();
        }

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
    }
}
