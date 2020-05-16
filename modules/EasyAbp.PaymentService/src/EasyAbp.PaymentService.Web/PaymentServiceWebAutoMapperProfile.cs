using EasyAbp.PaymentService.Refunds.Dtos;
using AutoMapper;
using EasyAbp.PaymentService.Payments.Dtos;

namespace EasyAbp.PaymentService.Web
{
    public class PaymentServiceWebAutoMapperProfile : Profile
    {
        public PaymentServiceWebAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<PaymentDto, CreatePaymentDto>();
            CreateMap<RefundDto, CreateRefundDto>();
            CreateMap<PaymentItemDto, CreatePaymentItemDto>();
        }
    }
}
