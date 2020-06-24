using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.Payments
{
    public class RefundPaymentEventHandler : IRefundPaymentEventHandler, ITransientDependency
    {
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentRepository _paymentRepository;

        public RefundPaymentEventHandler(
            IPaymentManager paymentManager,
            IPaymentRepository paymentRepository)
        {
            _paymentManager = paymentManager;
            _paymentRepository = paymentRepository;
        }
        
        [UnitOfWork(true)]
        public async Task HandleEventAsync(RefundPaymentEto eventData)
        {
            var payment = await _paymentRepository.GetAsync(eventData.PaymentId);

            await _paymentManager.StartRefundAsync(payment, eventData.Items.Select(etoItem => new RefundInfoModel
            {
                RefundAmount = etoItem.RefundAmount,
                PaymentItem = payment.PaymentItems.Single(paymentItem => etoItem.PaymentItemId == paymentItem.Id),
                CustomerRemark = etoItem.CustomerRemark,
                StaffRemark = etoItem.StaffRemark
            }), eventData.DisplayReason);
        }
    }
}