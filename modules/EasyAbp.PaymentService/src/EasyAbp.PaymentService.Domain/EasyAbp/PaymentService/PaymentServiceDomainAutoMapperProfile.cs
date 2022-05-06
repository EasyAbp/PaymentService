using AutoMapper;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.PaymentService
{
    public class PaymentServiceDomainAutoMapperProfile : Profile
    {
        public PaymentServiceDomainAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<Payment, PaymentEto>().MapExtraProperties(MappingPropertyDefinitionChecks.None);
            CreateMap<PaymentItem, PaymentItemEto>().MapExtraProperties(MappingPropertyDefinitionChecks.None);
            CreateMap<Refund, RefundEto>().MapExtraProperties(MappingPropertyDefinitionChecks.None);
            CreateMap<RefundItem, RefundItemEto>().MapExtraProperties(MappingPropertyDefinitionChecks.None);
        }
    }
}
