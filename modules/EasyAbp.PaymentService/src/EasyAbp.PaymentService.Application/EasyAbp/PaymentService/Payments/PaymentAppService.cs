using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Authorization;
using EasyAbp.PaymentService.Localization;
using EasyAbp.PaymentService.Payments.Dtos;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.Payments
{
    public class PaymentAppService : CrudAppService<Payment, PaymentDto, Guid, PagedAndSortedResultRequestDto, object, object>,
        IPaymentAppService
    {
        protected override string GetPolicyName { get; set; } = PaymentServicePermissions.Payments.Default;
        protected override string GetListPolicyName { get; set; } = PaymentServicePermissions.Payments.Default;

        private readonly IStringLocalizer<PaymentServiceResource> _stringLocalizer;
        private readonly IPaymentServiceResolver _paymentServiceResolver;
        private readonly IPaymentRepository _repository;

        public PaymentAppService(
            IStringLocalizer<PaymentServiceResource> stringLocalizer,
            IPaymentServiceResolver paymentServiceResolver,
            IPaymentRepository repository) : base(repository)
        {
            _stringLocalizer = stringLocalizer;
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

        [RemoteService(false)]
        public override Task<PaymentDto> CreateAsync(object input)
        {
            throw new NotSupportedException();
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

        public virtual Task<ListResultDto<PaymentMethodDto>> GetListPaymentMethod()
        {
            var paymentMethods = _paymentServiceResolver.GetPaymentMethods().Select(paymentMethod =>
                new PaymentMethodDto
                {
                    PaymentMethod = paymentMethod,
                    Name = _stringLocalizer[paymentMethod]
                }).ToList();

            // Todo: cache the result.
            return Task.FromResult(new ListResultDto<PaymentMethodDto>(paymentMethods));
        }

        public virtual async Task<PaymentDto> PayAsync(PayDto input)
        {
            var payment = await _repository.GetAsync(input.PaymentId);
            
            var providerType = _paymentServiceResolver.GetProviderTypeOrDefault(payment.PaymentMethod) ??
                               throw new UnknownPaymentMethodException(payment.PaymentMethod);

            var provider = ServiceProvider.GetService(providerType) as IPaymentServiceProvider ??
                           throw new UnknownPaymentMethodException(payment.PaymentMethod);
            
            var configurations = await GetPayeeConfigurationsAsync(payment);

            foreach (var property in input.ExtraProperties)
            {
                configurations.AddIfNotContains(new KeyValuePair<string, object>(property.Key, property.Value));
            }
            
            // Todo: payment discount

            await provider.PayAsync(payment, configurations);

            return MapToGetOutputDto(payment);
        }
        
        protected virtual Task<Dictionary<string, object>> GetPayeeConfigurationsAsync(Payment payment)
        {
            // Todo: use payee configurations provider.
            // Todo: get store side payee configurations.
            
            var payeeConfigurations = new Dictionary<string, object>();
            
            return Task.FromResult(payeeConfigurations);
        }
    }
}