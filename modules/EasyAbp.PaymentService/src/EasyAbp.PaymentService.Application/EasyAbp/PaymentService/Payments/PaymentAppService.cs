using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Authorization;
using EasyAbp.PaymentService.Localization;
using EasyAbp.PaymentService.Payments.Dtos;
using EasyAbp.PaymentService.Refunds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace EasyAbp.PaymentService.Payments
{
    [Authorize]
    public class PaymentAppService : ReadOnlyAppService<Payment, PaymentDto, Guid, PagedAndSortedResultRequestDto>,
        IPaymentAppService
    {
        protected override string GetPolicyName { get; set; } = PaymentServicePermissions.Payments.Default;
        protected override string GetListPolicyName { get; set; } = PaymentServicePermissions.Payments.Default;

        private readonly IPaymentManager _paymentManager;
        private readonly IStringLocalizer<PaymentServiceResource> _stringLocalizer;
        private readonly IPaymentServiceResolver _paymentServiceResolver;
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentRepository _repository;

        public PaymentAppService(
            IPaymentManager paymentManager,
            IStringLocalizer<PaymentServiceResource> stringLocalizer,
            IPaymentServiceResolver paymentServiceResolver,
            IRefundRepository refundRepository,
            IPaymentRepository repository) : base(repository)
        {
            _paymentManager = paymentManager;
            _stringLocalizer = stringLocalizer;
            _paymentServiceResolver = paymentServiceResolver;
            _refundRepository = refundRepository;
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

        [AllowAnonymous]
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

        public virtual async Task<PaymentDto> PayAsync(Guid id, PayInput input)
        {
            var payment = await _repository.GetAsync(id);

            if (payment.UserId != CurrentUser.GetId())
            {
                throw new UsingUnauthorizedPaymentException(CurrentUser.GetId(), payment.Id);
            }
            
            var configurations = await GetPayeeConfigurationsAsync(payment);
            
            foreach (var property in input.ExtraProperties)
            {
                configurations.AddIfNotContains(new KeyValuePair<string, object>(property.Key, property.Value));
            }

            await _paymentManager.StartPaymentAsync(payment, configurations);

            return MapToGetOutputDto(payment);
        }

        public virtual async Task<PaymentDto> CancelAsync(Guid id)
        {
            var payment = await _repository.GetAsync(id);

            if (payment.UserId != CurrentUser.GetId() &&
                !await AuthorizationService.IsGrantedAsync(PaymentServicePermissions.Payments.Manage))
            {
                throw new UsingUnauthorizedPaymentException(CurrentUser.GetId(), payment.Id);
            }

            await _paymentManager.StartCancelAsync(payment);

            return MapToGetOutputDto(payment);
        }

        [Authorize(PaymentServicePermissions.Payments.Manage)]
        public async Task<PaymentDto> RefundRollbackAsync(Guid id)
        {
            var payment = await _repository.GetAsync(id);

            if (payment.PendingRefundAmount <= decimal.Zero)
            {
                throw new PaymentIsInUnexpectedStageException(payment.Id);
            }

            var refund = await _refundRepository.FindByPaymentIdAsync(payment.Id);

            await _paymentManager.RollbackRefundAsync(payment, refund);

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