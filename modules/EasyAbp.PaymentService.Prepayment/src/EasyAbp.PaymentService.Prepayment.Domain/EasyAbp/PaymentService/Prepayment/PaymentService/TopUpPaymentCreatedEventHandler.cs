using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Prepayment.Accounts;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.Prepayment.PaymentService
{
    public class TopUpPaymentCreatedEventHandler : IDistributedEventHandler<EntityCreatedEto<PaymentEto>>, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IAccountRepository _accountRepository;

        public TopUpPaymentCreatedEventHandler(
            ICurrentTenant currentTenant,
            IAccountRepository accountRepository)
        {
            _currentTenant = currentTenant;
            _accountRepository = accountRepository;
        }
        
        [UnitOfWork(true)]
        public virtual async Task HandleEventAsync(EntityCreatedEto<PaymentEto> eventData)
        {
            var payment = eventData.Entity;

            var items = payment.PaymentItems.Where(item => item.ItemType == PrepaymentConsts.TopUpPaymentItemType)
                .ToList();

            foreach (var item in items)
            {
                var accountId = item.ItemKey;

                using var currentTenant = _currentTenant.Change(payment.TenantId);

                var account = await _accountRepository.GetAsync(accountId);
            
                account.SetPendingTopUpPaymentId(payment.Id);

                await _accountRepository.UpdateAsync(account, true);
            }
        }
    }
}