using System;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class WeChatPayFeeConverter : IWeChatPayFeeConverter, ITransientDependency
    {
        public virtual int ConvertToWeChatPayFee(decimal fee)
        {
            return Convert.ToInt32(decimal.Round(fee, 2, MidpointRounding.AwayFromZero) * 100);
        }

        public virtual decimal ConvertToDecimalFee(int fee)
        {
            return Convert.ToDecimal(fee / 100);
        }
    }
}