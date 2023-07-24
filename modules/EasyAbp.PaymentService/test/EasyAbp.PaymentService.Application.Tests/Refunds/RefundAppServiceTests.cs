using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.PaymentService.Refunds
{
    public class RefundAppServiceTests : PaymentServiceApplicationTestBase
    {
        private readonly IRefundAppService _refundAppService;

        public RefundAppServiceTests()
        {
            _refundAppService = GetRequiredService<IRefundAppService>();
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
