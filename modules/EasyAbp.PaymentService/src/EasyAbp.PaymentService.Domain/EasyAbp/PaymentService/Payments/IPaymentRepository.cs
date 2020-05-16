using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentRepository : IRepository<Payment, Guid>
    {
    }
}