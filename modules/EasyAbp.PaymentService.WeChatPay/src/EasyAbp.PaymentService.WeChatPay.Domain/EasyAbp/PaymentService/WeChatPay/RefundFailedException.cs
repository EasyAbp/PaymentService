using Volo.Abp;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class RefundFailedException : BusinessException
    {
        public RefundFailedException() : base(message: $"Refund failed")
        {
        }
        
        public RefundFailedException(string returnCode, string returnMsg) : base(
            message: $"Refund failed, return_code: {returnCode}, return_msg: {returnMsg}")
        {
        }

        public RefundFailedException(string returnCode, string returnMsg, string errCode, string errCodeDes) :
            base(message: $"Refund failed, return_code: {returnCode}, return_msg: {returnMsg}, err_code: {errCode}, err_code_des: {errCodeDes}")
        {
        }
    }
}