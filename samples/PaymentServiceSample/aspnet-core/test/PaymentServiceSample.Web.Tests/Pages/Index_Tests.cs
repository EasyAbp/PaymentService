using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace PaymentServiceSample.Pages
{
    public class Index_Tests : PaymentServiceSampleWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
