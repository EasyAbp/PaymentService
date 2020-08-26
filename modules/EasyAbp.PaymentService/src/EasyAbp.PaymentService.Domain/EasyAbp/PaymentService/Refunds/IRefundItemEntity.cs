using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.PaymentService.Refunds
{
    public interface IRefundItemEntity : IRefundItem, IEntity<Guid>, IHasExtraProperties
    {
    }
}