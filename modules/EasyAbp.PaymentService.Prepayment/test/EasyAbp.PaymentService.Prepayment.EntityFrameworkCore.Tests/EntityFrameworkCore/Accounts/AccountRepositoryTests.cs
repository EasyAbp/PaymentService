using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Prepayment.Accounts;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.PaymentService.Prepayment.EntityFrameworkCore.Accounts
{
    public class AccountRepositoryTests : PrepaymentEntityFrameworkCoreTestBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountRepositoryTests()
        {
            _accountRepository = GetRequiredService<IAccountRepository>();
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
