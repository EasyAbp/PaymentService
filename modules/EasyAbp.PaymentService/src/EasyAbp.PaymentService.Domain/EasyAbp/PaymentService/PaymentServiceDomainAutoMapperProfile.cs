using AutoMapper;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;

namespace EasyAbp.PaymentService
{
    public class PaymentServiceDomainAutoMapperProfile : Profile
    {
        public PaymentServiceDomainAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<Payment, PaymentEto>();
            CreateMap<PaymentItem, PaymentItemEto>();
            CreateMap<Refund, RefundEto>();
        }
    }
}
