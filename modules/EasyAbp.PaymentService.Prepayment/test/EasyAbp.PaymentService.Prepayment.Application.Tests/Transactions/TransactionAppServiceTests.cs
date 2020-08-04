using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.PaymentService.Prepayment.Transactions
{
    public class TransactionAppServiceTests : PrepaymentApplicationTestBase
    {
        private readonly ITransactionAppService _transactionAppService;

        public TransactionAppServiceTests()
        {
            _transactionAppService = GetRequiredService<ITransactionAppService>();
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
