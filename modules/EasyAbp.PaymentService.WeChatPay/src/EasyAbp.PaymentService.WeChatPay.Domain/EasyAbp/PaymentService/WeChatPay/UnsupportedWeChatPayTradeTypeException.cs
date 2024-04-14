using Volo.Abp;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class UnsupportedWeChatPayTradeTypeException : BusinessException
    {
        public UnsupportedWeChatPayTradeTypeException(string tradeType) : base(
            message: $"Unsupported trade_type: {tradeType}.")
        {
        }
    }
}