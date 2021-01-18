using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Payments.Dtos;
using EasyAbp.PaymentService.Refunds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
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
            await _distributedEventBus.PublishAsync(new CreatePaymentEto(
                CurrentTenant.Id,
                CurrentUser.GetId(),
                "Free",
                "CNY",
                new List<CreatePaymentItemEto>(new[]
                {
                    new CreatePaymentItemEto
                    {
                        ItemType = "Test",
                        ItemKey = Guid.NewGuid().ToString(),
                        OriginalPaymentAmount = 0
                    }
                })));
            
            return (await _paymentAppService.GetListAsync(new PagedAndSortedResultRequestDto())).Items.FirstOrDefault();
        }
        
        [Authorize]
        [Route("refund")]
        [HttpPost]
        public async Task<PaymentDto> RefundAsync(Guid paymentId)
        {
            var payment = await _paymentAppService.GetAsync(paymentId);

            await _distributedEventBus.PublishAsync(new RefundPaymentEto(CurrentTenant.Id, new CreateRefundInput
                {
                    PaymentId = payment.Id,
                    DisplayReason = "Test0",
                    CustomerRemark = "Test1",
                    StaffRemark = "Test2",
                    RefundItems = new List<CreateRefundItemInput>(payment.PaymentItems
                        .Where(item => item.ActualPaymentAmount > 0).Select(item => new CreateRefundItemInput()
                        {
                            PaymentItemId = item.Id,
                            RefundAmount = item.ActualPaymentAmount,
                            CustomerRemark = "Test3",
                            StaffRemark = "Test4"
                        }))
                }
            ));

            return await _paymentAppService.GetAsync(paymentId);
        }
    }
}