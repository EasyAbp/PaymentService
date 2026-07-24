using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace EasyAbp.PaymentService
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PaymentToPaymentEtoMapper : MapperBase<Payment, PaymentEto>
    {
        public override partial PaymentEto Map(Payment source);
        public override partial void Map(Payment source, PaymentEto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PaymentItemToPaymentItemEtoMapper : MapperBase<PaymentItem, PaymentItemEto>
    {
        public override partial PaymentItemEto Map(PaymentItem source);
        public override partial void Map(PaymentItem source, PaymentItemEto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class RefundToRefundEtoMapper : MapperBase<Refund, RefundEto>
    {
        public override partial RefundEto Map(Refund source);
        public override partial void Map(Refund source, RefundEto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class RefundItemToRefundItemEtoMapper : MapperBase<RefundItem, RefundItemEto>
    {
        public override partial RefundItemEto Map(RefundItem source);
        public override partial void Map(RefundItem source, RefundItemEto destination);
    }
}
