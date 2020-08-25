using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.Refunds.Dtos;
using AutoMapper;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Payments.Dtos;
using Volo.Abp.AutoMapper;
using RefundDto = EasyAbp.PaymentService.Refunds.Dtos.RefundDto;

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
            CreateMap<PaymentItem, PaymentItemDto>();
            CreateMap<Refund, RefundDto>();
            CreateMap<RefundItem, RefundItemDto>();
        }
    }
}
