using Shouldly;
using Xunit;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class WeChatPayFeeConverterTests
    {
        private readonly WeChatPayFeeConverter _converter = new();

        [Theory]
        [InlineData(1999, "19.99")]
        [InlineData(1, "0.01")]
        [InlineData(100, "1")]
        [InlineData(0, "0")]
        public void Should_Keep_The_Cents_When_Converting_To_Decimal(int fee, string expected)
        {
            _converter.ConvertToDecimalFee(fee).ShouldBe(decimal.Parse(expected));
        }

        [Theory]
        [InlineData("19.99", 1999)]
        [InlineData("0.015", 2)]
        [InlineData("1", 100)]
        public void Should_Convert_To_WeChat_Pay_Fee_In_Cents(string fee, int expected)
        {
            _converter.ConvertToWeChatPayFee(decimal.Parse(fee)).ShouldBe(expected);
        }

        [Fact]
        public void Should_Round_Trip()
        {
            _converter.ConvertToDecimalFee(_converter.ConvertToWeChatPayFee(123.45m)).ShouldBe(123.45m);
        }
    }
}
