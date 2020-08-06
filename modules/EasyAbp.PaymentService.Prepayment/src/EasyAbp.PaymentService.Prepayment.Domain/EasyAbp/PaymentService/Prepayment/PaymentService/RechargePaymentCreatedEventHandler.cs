using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Prepayment.Accounts;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.Prepayment.PaymentService
{
    public class RechargePaymentCreatedEventHandler : IDistributedEventHandler<EntityCreatedEto<PaymentEto>>
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IAccountRepository _accountRepository;

        public RechargePaymentCreatedEventHandler(
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

            var items = payment.PaymentItems.Where(item => item.ItemType == PrepaymentConsts.RechargePaymentItemType)
                .ToList();

            if (items.IsNullOrEmpty())
            {
                return;
            }

            if (!Guid.TryParse(payment.GetProperty<string>("AccountId"), out var accountId))
            {
                throw new ArgumentNullException("AccountId");
            }

            using var currentTenant = _currentTenant.Change(payment.TenantId);

            var account = await _accountRepository.GetAsync(accountId);
            
            account.SetPendingRechargePaymentId(payment.Id);

            await _accountRepository.UpdateAsync(account, true);
        }
    }
}