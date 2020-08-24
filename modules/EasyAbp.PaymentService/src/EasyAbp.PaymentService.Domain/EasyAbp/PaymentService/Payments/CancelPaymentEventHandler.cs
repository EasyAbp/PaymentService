using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.Payments
{
    public class CancelPaymentEventHandler : ICancelPaymentEventHandler, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentManager _paymentManager;

        public CancelPaymentEventHandler(
            ICurrentTenant currentTenant,
            IPaymentRepository paymentRepository,
            IPaymentManager paymentManager)
        {
            _currentTenant = currentTenant;
            _paymentRepository = paymentRepository;
            _paymentManager = paymentManager;
        }
        
        [UnitOfWork(true)]
        public virtual async Task HandleEventAsync(CancelPaymentEto eventData)
        {
            using var changeTenant = _currentTenant.Change(eventData.TenantId);
            
            var payment = await _paymentRepository.GetAsync(eventData.PaymentId);
            
            await _paymentManager.StartCancelAsync(payment);
        }
    }
}