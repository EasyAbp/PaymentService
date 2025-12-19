using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.WeChatPay.Authorization;
using EasyAbp.PaymentService.WeChatPay.Localization;
using EasyAbp.PaymentService.WeChatPay.RefundRecords.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.WeChatPay.RefundRecords
{
    public class RefundRecordAppService : ReadOnlyAppService<RefundRecord, RefundRecordDto, Guid, PagedAndSortedResultRequestDto>,
        IRefundRecordAppService
    {
        protected override string GetPolicyName { get; set; } = WeChatPayPermissions.RefundRecords.Default;
        protected override string GetListPolicyName { get; set; } = WeChatPayPermissions.RefundRecords.Default;
        
        private readonly IRefundRecordRepository _repository;

        public RefundRecordAppService(IRefundRecordRepository repository) : base(repository)
        {
            _repository = repository;

            LocalizationResource = typeof(WeChatPayResource);
            ObjectMapperContext = typeof(PaymentServiceWeChatPayApplicationModule);
        }
    }
}