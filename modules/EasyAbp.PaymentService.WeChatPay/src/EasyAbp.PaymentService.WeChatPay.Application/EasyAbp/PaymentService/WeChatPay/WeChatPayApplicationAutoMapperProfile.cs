using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using EasyAbp.PaymentService.WeChatPay.RefundRecords.Dtos;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords.Dtos;
using AutoMapper;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class WeChatPayApplicationAutoMapperProfile : Profile
    {
        public WeChatPayApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<RefundRecord, RefundRecordDto>();
            CreateMap<PaymentRecord, PaymentRecordDto>();
        }
    }
}
