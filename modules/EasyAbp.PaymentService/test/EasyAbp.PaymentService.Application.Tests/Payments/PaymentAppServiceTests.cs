using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Payments.Dtos;
using Shouldly;
using Volo.Abp.EventBus.Distributed;
using Xunit;

namespace Payments
{
    public class PaymentAppServiceTests : PaymentServiceApplicationTestBase
    {
        private readonly Guid _userId = Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb0d");

        private readonly IDistributedEventBus _distributedEventBus;
        private readonly IPaymentAppService _paymentAppService;

        public PaymentAppServiceTests()
        {
            _distributedEventBus = GetRequiredService<IDistributedEventBus>();
            _paymentAppService = GetRequiredService<IPaymentAppService>();
        }

        [Fact]
        public async Task<Guid> Should_Create_Payment()
        {
            var itemKey = Guid.NewGuid().ToString();
            
            await _distributedEventBus.PublishAsync(new CreatePaymentEto(
                null,
                _userId, 
                "Free",
                "CNY",
                new List<CreatePaymentItemEto>(new[]
                {
                    new CreatePaymentItemEto
                    {
                        ItemType = "Test",
                        ItemKey = itemKey,
                        OriginalPaymentAmount = 0
                    }
                })), false, false);
            
            var payments = await _paymentAppService.GetListAsync(new GetPaymentListInput());
            payments.Items.Count.ShouldBe(1);
            var payment = payments.Items[0];
            payment.UserId.ShouldBe(_userId);
            payment.PaymentMethod.ShouldBe("Free");
            payment.Currency.ShouldBe("CNY");
            payment.OriginalPaymentAmount.ShouldBe(decimal.Zero);
            payment.ActualPaymentAmount.ShouldBe(decimal.Zero);
            payment.RefundAmount.ShouldBe(decimal.Zero);
            payment.PendingRefundAmount.ShouldBe(decimal.Zero);
            payment.CanceledTime.ShouldBeNull();
            payment.CompletionTime.ShouldBeNull();
            payment.PaymentItems.Count.ShouldBe(1);
            payment.PaymentItems[0].ItemType.ShouldBe("Test");
            payment.PaymentItems[0].ItemKey.ShouldBe(itemKey);
            payment.PaymentItems[0].OriginalPaymentAmount.ShouldBe(decimal.Zero);
            payment.PaymentItems[0].ActualPaymentAmount.ShouldBe(decimal.Zero);
            payment.PaymentItems[0].PaymentDiscount.ShouldBe(decimal.Zero);
            payment.PaymentItems[0].RefundAmount.ShouldBe(decimal.Zero);
            payment.PaymentItems[0].PendingRefundAmount.ShouldBe(decimal.Zero);

            return payment.Id;
        }

        [Fact]
        public async Task<Guid> Should_Complete_Payment()
        {
            var id = await Should_Create_Payment();

            var payment = await _paymentAppService.PayAsync(id, new PayInput());
            
            payment.CompletionTime.ShouldNotBeNull();

            return id;
        }

        [Fact]
        public async Task Should_Cancel_Payment()
        {
            var id = await Should_Create_Payment();
            
            var payment = await _paymentAppService.CancelAsync(id);

            payment.CanceledTime.ShouldNotBeNull();
            payment.CompletionTime.ShouldBeNull();
        }
        
        [Fact]
        public async Task Should_Not_Allowed_To_Cancel_Completed_Payments()
        {
            var id = await Should_Complete_Payment();

            await Assert.ThrowsAsync<PaymentIsInUnexpectedStageException>(async () =>
            {
                await _paymentAppService.CancelAsync(id);
            });
        }
    }
}
