using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.Payments
{
    public class CreatePaymentEventHandler : ICreatePaymentEventHandler, ITransientDependency
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentServiceResolver _paymentServiceResolver;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGuidGenerator _guidGenerator;

        public CreatePaymentEventHandler(
            IPaymentRepository paymentRepository,
            IPaymentServiceResolver paymentServiceResolver,
            IServiceProvider serviceProvider,
            IGuidGenerator guidGenerator)
        {
            _paymentRepository = paymentRepository;
            _paymentServiceResolver = paymentServiceResolver;
            _serviceProvider = serviceProvider;
            _guidGenerator = guidGenerator;
        }
        
        [UnitOfWork(true)]
        public virtual async Task HandleEventAsync(CreatePaymentEto eventData)
        {
            var providerType = _paymentServiceResolver.GetProviderTypeOrDefault(eventData.PaymentMethod) ??
                               throw new UnknownPaymentMethodException(eventData.PaymentMethod);

            var provider = _serviceProvider.GetService(providerType) as IPaymentServiceProvider ??
                           throw new UnknownPaymentMethodException(eventData.PaymentMethod);

            var paymentItems = eventData.PaymentItems.Select(inputPaymentItem =>
                new PaymentItem(_guidGenerator.Create(), inputPaymentItem.ItemType, inputPaymentItem.ItemKey,
                    inputPaymentItem.Currency, inputPaymentItem.OriginalPaymentAmount)).ToList();

            if (paymentItems.Select(item => item.Currency).Any(c => c != eventData.Currency))
            {
                throw new MultiCurrencyNotSupportedException();
            }

            if (await HasDuplicatePaymentItemInProgressAsync(paymentItems))
            {
                throw new DuplicatePaymentRequestException();
            }

            var payment = new Payment(_guidGenerator.Create(), eventData.TenantId, eventData.UserId,
                eventData.PaymentMethod, eventData.Currency, paymentItems.Select(item => item.OriginalPaymentAmount).Sum(),
                paymentItems);
            
            foreach (var property in eventData.ExtraProperties)
            {
                payment.SetProperty(property.Key, property.Value);
            }
            
            await _paymentRepository.InsertAsync(payment, autoSave: true);
        }
        
        protected virtual async Task<bool> HasDuplicatePaymentItemInProgressAsync(IEnumerable<PaymentItem> paymentItems)
        {
            foreach (var item in paymentItems)
            {
                if (await _paymentRepository.FindPaymentInProgressByPaymentItem(item.ItemType, item.ItemKey) != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}