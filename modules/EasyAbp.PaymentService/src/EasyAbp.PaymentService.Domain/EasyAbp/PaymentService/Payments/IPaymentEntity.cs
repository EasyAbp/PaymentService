using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentEntity : IPayment, IAggregateRoot<Guid>, IMultiTenant
    {
    }
}