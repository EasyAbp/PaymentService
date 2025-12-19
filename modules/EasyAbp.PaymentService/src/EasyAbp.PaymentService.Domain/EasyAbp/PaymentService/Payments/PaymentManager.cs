using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentManager : DomainService, IPaymentManager
    {
        private readonly IClock _clock;
        private readonly IObjectMapper _objectMapper;
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentServiceResolver _paymentServiceResolver;
        private readonly IDistributedEventBus _distributedEventBus;

        public PaymentManager(
            IClock clock,
            IObjectMapper<PaymentServiceDomainModule> objectMapper,
            IRefundRepository refundRepository,
            IPaymentRepository paymentRepository,
            IPaymentServiceResolver paymentServiceResolver,
            IDistributedEventBus distributedEventBus)
        {
            _clock = clock;
            _objectMapper = objectMapper;
            _refundRepository = refundRepository;
            _paymentRepository = paymentRepository;
            _paymentServiceResolver = paymentServiceResolver;
            _distributedEventBus = distributedEventBus;
        }
        
        public virtual async Task StartPaymentAsync(Payment payment, ExtraPropertyDictionary configurations = null)
        {
            var provider = GetProvider(payment);

            // Todo: payment discount

            await provider.OnPaymentStartedAsync(payment, configurations);
        }

        [UnitOfWork(true)]
        public virtual async Task CompletePaymentAsync(Payment payment)
        {
            payment.CompletePayment(_clock.Now);
            
            await _paymentRepository.UpdateAsync(payment, true);
            
            await _distributedEventBus.PublishAsync(
                new PaymentCompletedEto(_objectMapper.Map<Payment, PaymentEto>(payment)));
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

        [UnitOfWork(true)]
        public virtual async Task CompleteCancelAsync(Payment payment)
        {
            payment.CancelPayment(_clock.Now);
            
            await _paymentRepository.UpdateAsync(payment, true);
            
            await _distributedEventBus.PublishAsync(
                new PaymentCanceledEto(_objectMapper.Map<Payment, PaymentEto>(payment)));
        }

        [UnitOfWork(true)]
        public virtual async Task StartRefundAsync(Payment payment, CreateRefundInput input)
        {
            var provider = GetProvider(payment);

            input.RefundItems.ForEach(x => x.RefundAmount.EnsureIsNonNegative());
            
            var refundAmount = input.RefundItems.Sum(x => x.RefundAmount);

            refundAmount.EnsureIsNonNegative();

            var paymentItemIds = input.RefundItems.Select(x => x.PaymentItemId).ToList();

            var exceptItemIds = paymentItemIds.Except(payment.PaymentItems.Select(x => x.Id)).ToList();
            
            if (exceptItemIds.Any())
            {
                throw new EntityNotFoundException(typeof(PaymentItem), exceptItemIds);
            }

            if (paymentItemIds.Count != paymentItemIds.Distinct().Count())
            {
                throw new DuplicatePaymentItemIdException();
            }

            if (await _refundRepository.FindByPaymentIdAsync(payment.Id) != null)
            {
                throw new AnotherRefundIsInProgressException(payment.Id);
            }

            var refund = CreateRefund(payment, input);

            await _refundRepository.InsertAsync(refund, true);
            
            payment.StartRefund(refund);
            
            await _paymentRepository.UpdateAsync(payment, true);

            await provider.OnRefundStartedAsync(payment, refund);
        }

        private Refund CreateRefund(Payment payment, CreateRefundInput input)
        {
            // Todo: other payment methods?
            var paymentMethod = payment.PaymentMethod;
            var currency = payment.Currency;

            var refundAmount = input.RefundItems.Sum(x => x.RefundAmount);

            var refundItems = input.RefundItems.Select(createRefundItemEto =>
                {
                    var refundItem = new RefundItem(GuidGenerator.Create(), createRefundItemEto.PaymentItemId,
                        createRefundItemEto.RefundAmount, createRefundItemEto.CustomerRemark,
                        createRefundItemEto.StaffRemark);
                    createRefundItemEto.MapExtraPropertiesTo(refundItem, MappingPropertyDefinitionChecks.None);
                    return refundItem;
                }
            ).ToList();

            var refund = new Refund(GuidGenerator.Create(), CurrentTenant.Id, payment.Id, paymentMethod, null, currency,
                refundAmount, input.DisplayReason, input.CustomerRemark, input.StaffRemark, refundItems);

            input.MapExtraPropertiesTo(refund, MappingPropertyDefinitionChecks.None);

            return refund;
        }

        [UnitOfWork(true)]
        public virtual async Task CompleteRefundAsync(Payment payment, Refund refund)
        {
            payment.CompleteRefund();
                
            await _paymentRepository.UpdateAsync(payment, true);
            
            refund.CompleteRefund(_clock.Now);

            await _refundRepository.UpdateAsync(refund, true);

            var paymentEto = _objectMapper.Map<Payment, PaymentEto>(payment);
            var refundEto = _objectMapper.Map<Refund, RefundEto>(refund);

            await _distributedEventBus.PublishAsync(new PaymentRefundCompletedEto(paymentEto, refundEto));
        }

        [UnitOfWork(true)]
        public virtual async Task RollbackRefundAsync(Payment payment, Refund refund)
        {
            payment.RollbackRefund();
            
            await _paymentRepository.UpdateAsync(payment, true);

            refund.CancelRefund(_clock.Now);

            await _refundRepository.UpdateAsync(refund, true);
            
            await _distributedEventBus.PublishAsync(new PaymentRefundRollbackEto(
                _objectMapper.Map<Payment, PaymentEto>(payment), _objectMapper.Map<Refund, RefundEto>(refund)));
        }

        protected virtual IPaymentServiceProvider GetProvider(IPayment payment)
        {
            var providerType = _paymentServiceResolver.GetProviderTypeOrDefault(payment.PaymentMethod) ??
                               throw new UnknownPaymentMethodException(payment.PaymentMethod);

            return LazyServiceProvider.LazyGetService(providerType) as IPaymentServiceProvider ??
                   throw new UnknownPaymentMethodException(payment.PaymentMethod);
        }
    }
}