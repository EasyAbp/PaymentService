using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.Payments
{
    public class RefundPaymentEventHandler : IRefundPaymentEventHandler, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentRepository _paymentRepository;

        public RefundPaymentEventHandler(
            ICurrentTenant currentTenant,
            IPaymentManager paymentManager,
            IPaymentRepository paymentRepository)
        {
            _currentTenant = currentTenant;
            _paymentManager = paymentManager;
            _paymentRepository = paymentRepository;
        }
        
        [UnitOfWork(true)]
        public async Task HandleEventAsync(RefundPaymentEto eventData)
        {
            using var changeTenant = _currentTenant.Change(eventData.TenantId);

            var payment = await _paymentRepository.GetAsync(eventData.CreateRefundInput.PaymentId);

            await _paymentManager.StartRefundAsync(payment, eventData.CreateRefundInput);
        }
    }
}