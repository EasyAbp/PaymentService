using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.PaymentService.WeChatPay.PaymentRecords
{
    public class PaymentRecordAppServiceTests : WeChatPayApplicationTestBase
    {
        private readonly IPaymentRecordAppService _paymentRecordAppService;

        public PaymentRecordAppServiceTests()
        {
            _paymentRecordAppService = GetRequiredService<IPaymentRecordAppService>();
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
