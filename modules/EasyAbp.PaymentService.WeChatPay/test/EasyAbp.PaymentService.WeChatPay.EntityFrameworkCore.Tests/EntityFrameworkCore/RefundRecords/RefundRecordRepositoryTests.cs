using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore.RefundRecords
{
    public class RefundRecordRepositoryTests : WeChatPayEntityFrameworkCoreTestBase
    {
        private readonly IRepository<RefundRecord, Guid> _refundRecordRepository;

        public RefundRecordRepositoryTests()
        {
            _refundRecordRepository = GetRequiredService<IRepository<RefundRecord, Guid>>();
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
