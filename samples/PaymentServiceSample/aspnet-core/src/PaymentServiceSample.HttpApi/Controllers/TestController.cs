using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Payments.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace PaymentServiceSample.Controllers
{
    [Area("paymentServiceSample")]
    [Route("api/app/test")]
    public class TestController : PaymentServiceSampleController
    {
        private readonly IDistributedEventBus _distributedEventBus;
        private readonly IPaymentAppService _paymentAppService;

        public TestController(
            IDistributedEventBus distributedEventBus,
            IPaymentAppService paymentAppService)
        {
            _distributedEventBus = distributedEventBus;
            _paymentAppService = paymentAppService;
        }
        
        [Authorize]
        [Route("freePayment")]
        [HttpPost]
        public async Task<PaymentDto> CreateFreePaymentAsync()
        {
            await _distributedEventBus.PublishAsync(new CreatePaymentEto
            {
                Currency = "CNY",
                ExtraProperties = new Dictionary<string, object>(),
                PaymentItems = new List<CreatePaymentItemEto>(new []
                {
                    new CreatePaymentItemEto
                    {
                        Currency = "CNY",
                        ItemType = "Test",
                        ItemKey = Guid.NewGuid(),
                        OriginalPaymentAmount = 0
                    }
                }),
                PaymentMethod = "Free",
                TenantId = CurrentTenant.Id,
                UserId = CurrentUser.GetId()
            });
            
            return (await _paymentAppService.GetListAsync(new PagedAndSortedResultRequestDto())).Items.FirstOrDefault();
        }
        
        [Authorize]
        [Route("refund")]
        [HttpPost]
        public async Task<PaymentDto> RefundAsync(Guid paymentId)
        {
            var payment = await _paymentAppService.GetAsync(paymentId);

            await _distributedEventBus.PublishAsync(new RefundPaymentEto
            {
                TenantId = CurrentTenant.Id,
                PaymentId = paymentId,
                Items = new List<RefundPaymentItemEto>(payment.PaymentItems.Where(item => item.ActualPaymentAmount > 0)
                    .Select(item => new RefundPaymentItemEto
                    {
                        PaymentItemId = item.Id,
                        RefundAmount = item.ActualPaymentAmount,
                        CustomerRemark = "Test1",
                        StaffRemark = "Test2"
                    })),
                DisplayReason = "Test",
                ExtraProperties = new Dictionary<string, object>(),
            });

            return await _paymentAppService.GetAsync(paymentId);
        }
    }
}