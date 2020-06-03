using System;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentItemEntity : IPaymentItem, IEntity<Guid>
    {
        
    }
}