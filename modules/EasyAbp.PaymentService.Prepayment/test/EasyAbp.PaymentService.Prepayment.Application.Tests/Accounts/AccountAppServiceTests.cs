using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.PaymentService.Prepayment.Accounts
{
    public class AccountAppServiceTests : PrepaymentApplicationTestBase
    {
        private readonly IAccountAppService _accountAppService;

        public AccountAppServiceTests()
        {
            _accountAppService = GetRequiredService<IAccountAppService>();
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
