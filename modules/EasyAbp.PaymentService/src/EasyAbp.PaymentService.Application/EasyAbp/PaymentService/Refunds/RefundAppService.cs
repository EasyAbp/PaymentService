using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Authorization;
using EasyAbp.PaymentService.Localization;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Payments.Dtos;
using EasyAbp.PaymentService.Refunds.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace EasyAbp.PaymentService.Refunds
{
    public class RefundAppService : ReadOnlyAppService<Refund, RefundDto, Guid, GetRefundListInput>,
        IRefundAppService
    {
        protected override string GetPolicyName { get; set; } = PaymentServicePermissions.Refunds.Default;
        protected override string GetListPolicyName { get; set; } = PaymentServicePermissions.Refunds.Default;
        
        private readonly IRefundRepository _repository;
        private readonly IPaymentRepository _paymentRepository;

        public RefundAppService(
            IRefundRepository repository,
            IPaymentRepository paymentRepository) : base(repository)
        {
            _repository = repository;
            _paymentRepository = paymentRepository;

            LocalizationResource = typeof(PaymentServiceResource);
            ObjectMapperContext = typeof(PaymentServiceApplicationModule);
        }
        
        public override async Task<RefundDto> GetAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            var entity = await GetEntityByIdAsync(id);

            var payment = await _paymentRepository.FindAsync(entity.PaymentId);

            if (payment == null || payment.UserId != CurrentUser.GetId())
            {
                await AuthorizationService.CheckAsync(PaymentServicePermissions.Refunds.Manage);
            }

            return await MapToGetOutputDtoAsync(entity);
        }

        public override async Task<PagedResultDto<RefundDto>> GetListAsync(GetRefundListInput input)
        {
            await CheckGetListPolicyAsync();

            if (!input.PaymentId.HasValue)
            {
                await AuthorizationService.CheckAsync(PaymentServicePermissions.Refunds.Manage);
            }
            else
            {
                var payment = await _paymentRepository.FindAsync(input.PaymentId.Value);

                if (payment == null || payment.UserId != CurrentUser.GetId())
                {
                    await AuthorizationService.CheckAsync(PaymentServicePermissions.Refunds.Manage);
                }
            }
            
            var query = await CreateFilteredQueryAsync(input);

            var totalCount = await AsyncExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);
            var entityDtos = await MapToGetListOutputDtosAsync(entities);

            return new PagedResultDto<RefundDto>(
                totalCount,
                entityDtos
            );
        }

        protected override async Task<IQueryable<Refund>> CreateFilteredQueryAsync(GetRefundListInput input)
        {
            return (await _repository.WithDetailsAsync())
                .WhereIf(input.PaymentId.HasValue, x => x.PaymentId == input.PaymentId.Value);
        }
    }
}