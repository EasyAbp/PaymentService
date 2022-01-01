using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;

namespace EasyAbp.PaymentService.WeChatPay
{
    public interface IWeChatPayRefundEventHandler : IDistributedEventHandler<WeChatPayRefundEto>
    {
        
    }
}