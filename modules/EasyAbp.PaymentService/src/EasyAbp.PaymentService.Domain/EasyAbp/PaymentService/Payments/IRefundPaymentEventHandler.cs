using Volo.Abp.EventBus.Distributed;

namespace EasyAbp.PaymentService.Payments
{
    public interface IRefundPaymentEventHandler : IDistributedEventHandler<RefundPaymentEto>
    {
        
    }
}