using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using Xunit;

namespace Payments
{
    public class PaymentAppServiceTests : PaymentServiceApplicationTestBase
    {
        private readonly IPaymentAppService _paymentAppService;

        public PaymentAppServiceTests()
        {
            _paymentAppService = GetRequiredService<IPaymentAppService>();
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
