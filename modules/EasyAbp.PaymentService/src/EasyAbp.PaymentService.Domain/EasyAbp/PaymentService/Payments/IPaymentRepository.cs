using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.PaymentService.Payments
{
    public interface IPaymentRepository : IRepository<Payment, Guid>
    {
        Task<Payment> FindPaymentInProgressByPaymentItem(string paymentItemType, Guid paymentItemKey);
    }
}