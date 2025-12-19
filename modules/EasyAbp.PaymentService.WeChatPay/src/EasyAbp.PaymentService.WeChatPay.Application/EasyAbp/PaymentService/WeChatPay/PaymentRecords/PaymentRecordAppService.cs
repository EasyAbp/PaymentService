using System;
using System.Threading.Tasks;
using EasyAbp.PaymentService.WeChatPay.Authorization;
using EasyAbp.PaymentService.WeChatPay.Localization;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.WeChatPay.PaymentRecords
{
    public class PaymentRecordAppService : ReadOnlyAppService<PaymentRecord, PaymentRecordDto, Guid, PagedAndSortedResultRequestDto>,
        IPaymentRecordAppService
    {
        protected override string GetPolicyName { get; set; } = WeChatPayPermissions.PaymentRecords.Default;
        protected override string GetListPolicyName { get; set; } = WeChatPayPermissions.PaymentRecords.Default;

        private readonly IPaymentRecordRepository _repository;

        public PaymentRecordAppService(IPaymentRecordRepository repository) : base(repository)
        {
            _repository = repository;

            LocalizationResource = typeof(WeChatPayResource);
            ObjectMapperContext = typeof(PaymentServiceWeChatPayApplicationModule);
        }
    }
}