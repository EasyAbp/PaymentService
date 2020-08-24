using Volo.Abp;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class WeChatPayBusinessErrorException : BusinessException
    {
        public WeChatPayBusinessErrorException(string outTradeNo, string errCode, string errCodeDes) : base(
            "WeChatPayBusinessError",
            $"There was an error in your request for the WeChatPay order (outTradeNo: {outTradeNo}): [{errCode}] {errCodeDes}")
        {
        }
    }
}