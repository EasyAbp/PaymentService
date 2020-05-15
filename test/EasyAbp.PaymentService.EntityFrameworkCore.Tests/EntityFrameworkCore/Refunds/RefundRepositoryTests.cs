using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EntityFrameworkCore.Refunds
{
    public class RefundRepositoryTests : PaymentServiceEntityFrameworkCoreTestBase
    {
        private readonly IRepository<Refund, Guid> _refundRepository;

        public RefundRepositoryTests()
        {
            _refundRepository = GetRequiredService<IRepository<Refund, Guid>>();
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
