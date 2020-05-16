using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EntityFrameworkCore.Payments
{
    public class PaymentRepositoryTests : PaymentServiceEntityFrameworkCoreTestBase
    {
        private readonly IRepository<Payment, Guid> _paymentRepository;

        public PaymentRepositoryTests()
        {
            _paymentRepository = GetRequiredService<IRepository<Payment, Guid>>();
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
