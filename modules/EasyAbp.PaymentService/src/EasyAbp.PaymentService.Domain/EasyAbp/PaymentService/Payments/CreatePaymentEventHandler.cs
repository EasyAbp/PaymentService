using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.Payments
{
    public class CreatePaymentEventHandler : ICreatePaymentEventHandler, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentServiceResolver _paymentServiceResolver;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGuidGenerator _guidGenerator;

        public CreatePaymentEventHandler(
            ICurrentTenant currentTenant,
            IPaymentRepository paymentRepository,
            IPaymentServiceResolver paymentServiceResolver,
            IServiceProvider serviceProvider,
            IGuidGenerator guidGenerator)
        {
            _currentTenant = currentTenant;
            _paymentRepository = paymentRepository;
            _paymentServiceResolver = paymentServiceResolver;
            _serviceProvider = serviceProvider;
            _guidGenerator = guidGenerator;
        }
        
        [UnitOfWork(true)]
        public virtual async Task HandleEventAsync(CreatePaymentEto eventData)
        {
            using var changeTenant = _currentTenant.Change(eventData.TenantId);

            var providerType = _paymentServiceResolver.GetProviderTypeOrDefault(eventData.PaymentMethod) ??
                               throw new UnknownPaymentMethodException(eventData.PaymentMethod);

            var provider = _serviceProvider.GetService(providerType) as IPaymentServiceProvider ??
                           throw new UnknownPaymentMethodException(eventData.PaymentMethod);

            var paymentItems = eventData.PaymentItems.Select(itemEto =>
                {
                    var item = new PaymentItem(_guidGenerator.Create(), itemEto.ItemType, itemEto.ItemKey,
                        itemEto.Currency, itemEto.OriginalPaymentAmount);

                    itemEto.MapExtraPropertiesTo(item, MappingPropertyDefinitionChecks.None);

                    return item;
                }
            ).ToList();

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

            eventData.MapExtraPropertiesTo(payment, MappingPropertyDefinitionChecks.None);

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