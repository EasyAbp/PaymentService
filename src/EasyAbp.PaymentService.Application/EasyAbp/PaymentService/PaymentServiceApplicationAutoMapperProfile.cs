using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.Refunds.Dtos;
using AutoMapper;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Payments.Dtos;
using Volo.Abp.AutoMapper;

namespace EasyAbp.PaymentService
{
    public class PaymentServiceApplicationAutoMapperProfile : Profile
    {
        public PaymentServiceApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<Payment, PaymentDto>();
            CreateMap<Refund, RefundDto>();
            CreateMap<CreateRefundDto, Refund>(MemberList.Source);
            CreateMap<PaymentItem, PaymentItemDto>();
        }
    }
}
