using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Refunds;
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
            using var uow = _unitOfWorkManager.Begin(isTransactional: true);

            uow.OnCompleted(async () => await _distributedEventBus.PublishAsync(new PaymentCompletedEto
            {
                Payment = _objectMapper.Map<Payment, PaymentEto>(payment)
            }));
            
            payment.CompletePayment(_clock.Now);
            
            await _paymentRepository.UpdateAsync(payment, true);

            await uow.CompleteAsync();
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
            using var uow = _unitOfWorkManager.Begin(isTransactional: true);

            uow.OnCompleted(async () => await _distributedEventBus.PublishAsync(new PaymentCanceledEto
            {
                Payment = _objectMapper.Map<Payment, PaymentEto>(payment)
            }));
            
            payment.CancelPayment(_clock.Now);
            
            await _paymentRepository.UpdateAsync(payment, true);

            await uow.CompleteAsync();
        }

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

        public virtual async Task CompleteRefundAsync(Payment payment, Refund refund)
        {
            using var uow = _unitOfWorkManager.Begin(isTransactional: true);

            payment.CompleteRefund();
                
            await _paymentRepository.UpdateAsync(payment, true);
            
            refund.CompleteRefund(_clock.Now);

            await _refundRepository.UpdateAsync(refund, true);

            var paymentEto = _objectMapper.Map<Payment, PaymentEto>(payment);
            var refundEto = _objectMapper.Map<Refund, RefundEto>(refund);

            uow.OnCompleted(async () => await _distributedEventBus.PublishAsync(new PaymentRefundCompletedEto
            {
                Payment = paymentEto,
                Refund = refundEto
            }));

            await uow.CompleteAsync();
        }

        public virtual async Task RollbackRefundAsync(Payment payment, Refund refund)
        {
            using var uow = _unitOfWorkManager.Begin(isTransactional: true);

            uow.OnCompleted(async () => await _distributedEventBus.PublishAsync(new PaymentRefundRollbackEto
            {
                Payment = _objectMapper.Map<Payment, PaymentEto>(payment),
                Refund = _objectMapper.Map<Refund, RefundEto>(refund)
            }));

            payment.RollbackRefund();
            
            await _paymentRepository.UpdateAsync(payment, true);

            refund.CancelRefund(_clock.Now);

            await _refundRepository.UpdateAsync(refund, true);

            await uow.CompleteAsync();
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