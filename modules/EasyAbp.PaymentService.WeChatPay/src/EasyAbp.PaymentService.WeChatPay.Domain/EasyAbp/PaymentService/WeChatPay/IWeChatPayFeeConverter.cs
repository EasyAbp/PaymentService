namespace EasyAbp.PaymentService.WeChatPay
{
    public interface IWeChatPayFeeConverter
    {
        int ConvertToWeChatPayFee(decimal fee);

        decimal ConvertToDecimalFee(int fee);
    }
}