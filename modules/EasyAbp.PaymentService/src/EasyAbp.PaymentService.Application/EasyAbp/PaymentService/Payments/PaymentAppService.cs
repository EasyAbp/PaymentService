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
using Volo.Abp.Data;
using Volo.Abp.Users;

namespace EasyAbp.PaymentService.Payments
{
    [Authorize]
    public class PaymentAppService : ReadOnlyAppService<Payment, PaymentDto, Guid, GetPaymentListInput>,
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

        public override async Task<PaymentDto> GetAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            var entity = await GetEntityByIdAsync(id);

            if (entity.UserId != CurrentUser.GetId())
            {
                await AuthorizationService.CheckAsync(PaymentServicePermissions.Payments.Manage.ManageDefault);
            }

            return await MapToGetOutputDtoAsync(entity);
        }

        public override async Task<PagedResultDto<PaymentDto>> GetListAsync(GetPaymentListInput input)
        {
            await CheckGetListPolicyAsync();

            if (!input.UserId.HasValue || input.UserId.Value != CurrentUser.GetId())
            {
                await AuthorizationService.CheckAsync(PaymentServicePermissions.Payments.Manage.ManageDefault);
            }
            
            var query = await CreateFilteredQueryAsync(input);

            var totalCount = await AsyncExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);
            var entityDtos = await MapToGetListOutputDtosAsync(entities);

            return new PagedResultDto<PaymentDto>(
                totalCount,
                entityDtos
            );
        }

        protected override async Task<IQueryable<Payment>> CreateFilteredQueryAsync(GetPaymentListInput input)
        {
            return (await _repository.WithDetailsAsync())
                .WhereIf(input.UserId.HasValue, x => x.UserId == input.UserId.Value);
        }

        [AllowAnonymous]
        public virtual Task<ListResultDto<PaymentMethodDto>> GetListPaymentMethodAsync()
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
                // todo: we should validate the extra properties in PayInput, see: https://github.com/abpframework/abp/discussions/15583
                configurations.AddIfNotContains(new KeyValuePair<string, object>(property.Key, property.Value));
            }

            await _paymentManager.StartPaymentAsync(payment, configurations);

            return await MapToGetOutputDtoAsync(payment);
        }

        public virtual async Task<PaymentDto> CancelAsync(Guid id)
        {
            var payment = await _repository.GetAsync(id);

            if (payment.UserId != CurrentUser.GetId() &&
                !await AuthorizationService.IsGrantedAsync(PaymentServicePermissions.Payments.Manage.Cancel))
            {
                throw new UsingUnauthorizedPaymentException(CurrentUser.GetId(), payment.Id);
            }

            await _paymentManager.StartCancelAsync(payment);

            return await MapToGetOutputDtoAsync(payment);
        }

        [Authorize(PaymentServicePermissions.Payments.Manage.RollbackRefund)]
        public async Task<PaymentDto> RefundRollbackAsync(Guid id)
        {
            var payment = await _repository.GetAsync(id);

            if (payment.PendingRefundAmount <= decimal.Zero)
            {
                throw new PaymentIsInUnexpectedStageException(payment.Id);
            }

            var refund = await _refundRepository.FindByPaymentIdAsync(payment.Id);

            await _paymentManager.RollbackRefundAsync(payment, refund);

            return await MapToGetOutputDtoAsync(payment);
        }

        protected virtual Task<ExtraPropertyDictionary> GetPayeeConfigurationsAsync(Payment payment)
        {
            // Todo: use payee configurations provider.
            // Todo: (EShop) get store side payee configurations. e.g. set `mch_id` for WeChatPay
            
            var payeeConfigurations = new ExtraPropertyDictionary();
            
            return Task.FromResult(payeeConfigurations);
        }
    }
}