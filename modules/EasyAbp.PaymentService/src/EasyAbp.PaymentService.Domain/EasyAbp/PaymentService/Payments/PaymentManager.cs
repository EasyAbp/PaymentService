using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentManager : DomainService, IPaymentManager
    {
        private readonly IClock _clock;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentServiceResolver _paymentServiceResolver;
        private readonly IDistributedEventBus _distributedEventBus;

        public PaymentManager(
            IClock clock,
            IObjectMapper objectMapper,
            IUnitOfWorkManager unitOfWorkManager,
            IRefundRepository refundRepository,
            IPaymentRepository paymentRepository,
            IPaymentServiceResolver paymentServiceResolver,
            IDistributedEventBus distributedEventBus)
        {
            _clock = clock;
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
            _refundRepository = refundRepository;
            _paymentRepository = paymentRepository;
            _paymentServiceResolver = paymentServiceResolver;
            _distributedEventBus = distributedEventBus;
        }
        
        public virtual async Task StartPaymentAsync(Payment payment, Dictionary<string, object> configurations = null)
        {
            var provider = GetProvider(payment);

            // Todo: payment discount

            await provider.OnPaymentStartedAsync(payment, configurations);
        }

        public virtual async Task CompletePaymentAsync(Payment payment)
        {
            _unitOfWorkManager.Current.OnCompleted(async () =>
            {
                await _distributedEventBus.PublishAsync(new PaymentCompletedEto
                {
                    Payment = _objectMapper.Map<Payment, PaymentEto>(payment)
                });
            });
            
            payment.CompletePayment(_clock.Now);
            
            await _paymentRepository.UpdateAsync(payment, true);
        }

        public virtual async Task StartCancelAsync(Payment payment)
        {
            if (!payment.IsInProgress())
            {
                throw new PaymentIsInUnexpectedStageException(payment.Id);
            }
            
            var provider = GetProvider(payment);
            
            await provider.OnCancelStartedAsync(payment);
        }

        public virtual async Task CompleteCancelAsync(Payment payment)
        {
            _unitOfWorkManager.Current.OnCompleted(async () =>
            {
                await _distributedEventBus.PublishAsync(new PaymentCancelCompletedEto
                {
                    Payment = _objectMapper.Map<Payment, PaymentEto>(payment)
                });
            });
            
            payment.CancelPayment(_clock.Now);
            
            await _paymentRepository.UpdateAsync(payment, true);
        }

        public virtual async Task StartRefundAsync(Payment payment, IEnumerable<RefundInfoModel> refundInfos, string displayReason = null)
        {
            var provider = GetProvider(payment);
            
            var refundInfoModels = refundInfos.ToList();
            
            payment.StartRefund(refundInfoModels);
            
            await _paymentRepository.UpdateAsync(payment, true);

            var refunds = new List<Refund>();

            foreach (var refund in refundInfoModels.Select(model => new Refund(
                id: GuidGenerator.Create(),
                tenantId: CurrentTenant.Id,
                paymentId: payment.Id,
                paymentItemId: model.PaymentItem.Id,
                refundPaymentMethod: payment.PaymentMethod,
                externalTradingCode: null,
                currency: payment.Currency,
                refundAmount: model.RefundAmount,
                customerRemark: model.CustomerRemark,
                staffRemark: model.StaffRemark
            )))
            {
                refunds.Add(await _refundRepository.InsertAsync(refund, true));
            }

            await provider.OnRefundStartedAsync(payment, refunds, displayReason);
        }

        public virtual async Task CompleteRefundAsync(Payment payment, IEnumerable<Refund> refunds)
        {
            _unitOfWorkManager.Current.OnCompleted(async () =>
            {
                await _distributedEventBus.PublishAsync(new PaymentRefundCompletedEto
                {
                    Payment = _objectMapper.Map<Payment, PaymentEto>(payment),
                    Refunds = _objectMapper.Map<IEnumerable<Refund>, IEnumerable<RefundEto>>(refunds)
                });
            });
            
            payment.CompleteRefund();
                
            await _paymentRepository.UpdateAsync(payment, true);
            
            foreach (var refund in refunds)
            {
                refund.CompleteRefund(_clock.Now);

                await _refundRepository.UpdateAsync(refund, true);
            }
        }

        public virtual async Task RollbackRefundAsync(Payment payment, IEnumerable<Refund> refunds)
        {
            _unitOfWorkManager.Current.OnCompleted(async () =>
            {
                await _distributedEventBus.PublishAsync(new PaymentRefundRollbackEto
                {
                    Payment = _objectMapper.Map<Payment, PaymentEto>(payment),
                    Refunds = _objectMapper.Map<IEnumerable<Refund>, IEnumerable<RefundEto>>(refunds)
                });
            });

            payment.RollbackRefund();
            
            await _paymentRepository.UpdateAsync(payment, true);

            foreach (var refund in refunds)
            {
                refund.CancelRefund(_clock.Now);

                await _refundRepository.UpdateAsync(refund, true);
            }
        }

        protected virtual IPaymentServiceProvider GetProvider(IPayment payment)
        {
            var providerType = _paymentServiceResolver.GetProviderTypeOrDefault(payment.PaymentMethod) ??
                               throw new UnknownPaymentMethodException(payment.PaymentMethod);

            return ServiceProvider.GetService(providerType) as IPaymentServiceProvider ??
                   throw new UnknownPaymentMethodException(payment.PaymentMethod);
        }
    }
}