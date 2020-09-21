using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRequests
{
    public class WithdrawalRequestAppServiceTests : PrepaymentApplicationTestBase
    {
        private readonly IWithdrawalRequestAppService _withdrawalRequestAppService;

        public WithdrawalRequestAppServiceTests()
        {
            _withdrawalRequestAppService = GetRequiredService<IWithdrawalRequestAppService>();
        }

        /*
        [Fact]
        public async Task Test1()
        {
            // Arrange

            // Act

            // Assert
        }
        */
    }
}
