using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore.PaymentRecords
{
    public class PaymentRecordRepositoryTests : WeChatPayEntityFrameworkCoreTestBase
    {
        private readonly IRepository<PaymentRecord, Guid> _paymentRecordRepository;

        public PaymentRecordRepositoryTests()
        {
            _paymentRecordRepository = GetRequiredService<IRepository<PaymentRecord, Guid>>();
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
