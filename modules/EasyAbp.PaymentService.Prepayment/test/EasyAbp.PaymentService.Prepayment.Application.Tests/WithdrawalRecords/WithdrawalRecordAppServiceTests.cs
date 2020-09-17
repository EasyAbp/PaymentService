using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRecords
{
    public class WithdrawalRecordAppServiceTests : PrepaymentApplicationTestBase
    {
        private readonly IWithdrawalRecordAppService _withdrawalRecordAppService;

        public WithdrawalRecordAppServiceTests()
        {
            _withdrawalRecordAppService = GetRequiredService<IWithdrawalRecordAppService>();
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
