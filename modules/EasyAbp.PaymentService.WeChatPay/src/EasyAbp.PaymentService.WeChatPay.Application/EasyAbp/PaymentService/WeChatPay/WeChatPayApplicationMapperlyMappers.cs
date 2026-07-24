using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords.Dtos;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using EasyAbp.PaymentService.WeChatPay.RefundRecords.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace EasyAbp.PaymentService.WeChatPay
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PaymentRecordToPaymentRecordDtoMapper : MapperBase<PaymentRecord, PaymentRecordDto>
    {
        public override partial PaymentRecordDto Map(PaymentRecord source);
        public override partial void Map(PaymentRecord source, PaymentRecordDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class RefundRecordToRefundRecordDtoMapper : MapperBase<RefundRecord, RefundRecordDto>
    {
        public override partial RefundRecordDto Map(RefundRecord source);
        public override partial void Map(RefundRecord source, RefundRecordDto destination);
    }
}
