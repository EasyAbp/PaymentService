using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.PaymentService.WeChatPay.RefundRecords
{
    public class RefundRecordAppServiceTests : WeChatPayApplicationTestBase
    {
        private readonly IRefundRecordAppService _refundRecordAppService;

        public RefundRecordAppServiceTests()
        {
            _refundRecordAppService = GetRequiredService<IRefundRecordAppService>();
        }

        [Fact]
        public async Task Test1()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
