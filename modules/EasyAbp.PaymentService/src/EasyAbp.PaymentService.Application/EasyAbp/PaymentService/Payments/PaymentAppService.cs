using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Authorization;
using EasyAbp.PaymentService.Payments.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Users;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentAppService : CrudAppService<Payment, PaymentDto, Guid, PagedAndSortedResultRequestDto, CreatePaymentDto, object>,
        IPaymentAppService
    {
        protected override string GetPolicyName { get; set; } = PaymentServicePermissions.Payments.Default;
        protected override string GetListPolicyName { get; set; } = PaymentServicePermissions.Payments.Default;
        
        private readonly IPaymentServiceResolver _paymentServiceResolver;
        private readonly IPaymentRepository _repository;

        public PaymentAppService(
            IPaymentServiceResolver paymentServiceResolver,
            IPaymentRepository repository) : base(repository)
        {
            _paymentServiceResolver = paymentServiceResolver;
            _repository = repository;
        }

        public override Task<PaymentDto> GetAsync(Guid id)
        {
            // Todo: Check permission.
            return base.GetAsync(id);
        }

        public override Task<PagedResultDto<PaymentDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            // Todo: Check permission.
            return base.GetListAsync(input);
        }

        public override async Task<PaymentDto> CreateAsync(CreatePaymentDto input)
        {
            await CheckCreatePolicyAsync();

            var providerType = _paymentServiceResolver.GetProviderTypeOrDefault(input.PaymentMethod) ??
                               throw new UnknownPaymentMethodException(input.PaymentMethod);

            var provider = ServiceProvider.GetService(providerType) as IPaymentServiceProvider ??
                           throw new UnknownPaymentMethodException(input.PaymentMethod);

            var paymentItems = input.PaymentItems.Select(inputPaymentItem =>
                new PaymentItem(GuidGenerator.Create(), inputPaymentItem.ItemType, inputPaymentItem.ItemKey,
                    inputPaymentItem.Currency, inputPaymentItem.OriginalPaymentAmount)).ToList();

            if (paymentItems.Select(item => item.Currency).Any(c => c != input.Currency))
            {
                throw new MultiCurrencyNotSupportedException();
            }

            if (await HasDuplicatePaymentItemInProgressAsync(paymentItems))
            {
                throw new DuplicatePaymentRequestException();
            }

            var payment = new Payment(GuidGenerator.Create(), CurrentTenant.Id, CurrentUser.GetId(),
                input.PaymentMethod, input.Currency, paymentItems.Select(item => item.OriginalPaymentAmount).Sum(),
                paymentItems);
            
            foreach (var property in input.ExtraProperties)
            {
                payment.SetProperty(property.Key, property.Value);
            }

            await Repository.InsertAsync(payment, autoSave: true);
            
            var payeeConfigurations = await GetPayeeConfigurationsAsync(payment);

            // Todo: payment discount

            await provider.PayAsync(payment, payeeConfigurations);

            return MapToGetOutputDto(payment);
        }

        protected virtual async Task<bool> HasDuplicatePaymentItemInProgressAsync(IEnumerable<PaymentItem> paymentItems)
        {
            foreach (var item in paymentItems)
            {
                if (await _repository.FindPaymentInProgressByPaymentItem(item.ItemType, item.ItemKey) != null)
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual Task<Dictionary<string, object>> GetPayeeConfigurationsAsync(Payment payment)
        {
            // Todo: use payee configurations provider.
            // Todo: get store side payee configurations.
            
            var payeeConfigurations = new Dictionary<string, object>();
            
            return Task.FromResult(payeeConfigurations);
        }

        [RemoteService(false)]
        public override Task<PaymentDto> UpdateAsync(Guid id, object input)
        {
            throw new NotSupportedException();
        }

        [RemoteService(false)]
        public override Task DeleteAsync(Guid id)
        {
            throw new NotSupportedException();
        }
    }
}