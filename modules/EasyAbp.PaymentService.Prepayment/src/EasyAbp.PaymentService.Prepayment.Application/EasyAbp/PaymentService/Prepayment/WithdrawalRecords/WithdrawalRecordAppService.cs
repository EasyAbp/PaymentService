using System;
using EasyAbp.PaymentService.Prepayment.Localization;
using EasyAbp.PaymentService.Prepayment.Permissions;
using EasyAbp.PaymentService.Prepayment.WithdrawalRecords.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.PaymentService.Prepayment.WithdrawalRecords
{
    public class WithdrawalRecordAppService : ReadOnlyAppService<WithdrawalRecord, WithdrawalRecordDto, Guid, PagedAndSortedResultRequestDto>,
        IWithdrawalRecordAppService
    {
        protected override string GetPolicyName { get; set; } = PrepaymentPermissions.WithdrawalRecord.Default;
        protected override string GetListPolicyName { get; set; } = PrepaymentPermissions.WithdrawalRecord.Default;

        private readonly IWithdrawalRecordRepository _repository;
        
        public WithdrawalRecordAppService(IWithdrawalRecordRepository repository) : base(repository)
        {
            _repository = repository;

            LocalizationResource = typeof(PrepaymentResource);
            ObjectMapperContext = typeof(PaymentServicePrepaymentApplicationModule);
        }
    }
}
