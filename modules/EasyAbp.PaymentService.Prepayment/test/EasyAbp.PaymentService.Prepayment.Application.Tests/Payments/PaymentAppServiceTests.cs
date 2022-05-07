using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Payments.Dtos;
using EasyAbp.PaymentService.Prepayment.Accounts;
using EasyAbp.PaymentService.Prepayment.PaymentService;
using EasyAbp.PaymentService.Prepayment.Transactions;
using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.Refunds.Dtos;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Xunit;

namespace EasyAbp.PaymentService.Prepayment.Payments
{
    public class PaymentAppServiceTests : PrepaymentApplicationTestBase
    {
        private readonly IDistributedEventBus _distributedEventBus;
        private readonly IPaymentAppService _paymentAppService;
        private readonly IRefundAppService _refundAppService;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public PaymentAppServiceTests()
        {
            _distributedEventBus = GetRequiredService<IDistributedEventBus>();
            _paymentAppService = GetRequiredService<IPaymentAppService>();
            _refundAppService = GetRequiredService<IRefundAppService>();
            _accountRepository = GetRequiredService<IAccountRepository>();
            _transactionRepository = GetRequiredService<ITransactionRepository>();
        }

        [Fact]
        public async Task<Guid> Should_Create_Payment()
        {
            var itemKey1 = Guid.NewGuid().ToString();
            var itemKey2 = Guid.NewGuid().ToString();
            
            await _distributedEventBus.PublishAsync(new CreatePaymentEto(
                null,
                PrepaymentTestConsts.UserId,
                PrepaymentPaymentServiceProvider.PaymentMethod,
                "CNY",
                new List<CreatePaymentItemEto>(new[]
                {
                    new CreatePaymentItemEto
                    {
                        ItemType = "Test",
                        ItemKey = itemKey1,
                        OriginalPaymentAmount = 50m
                    },
                    new CreatePaymentItemEto
                    {
                        ItemType = "Test",
                        ItemKey = itemKey2,
                        OriginalPaymentAmount = 100m
                    }
                })), false, false);
            
            var payments = await _paymentAppService.GetListAsync(new GetPaymentListInput());
            payments.Items.Count.ShouldBe(1);
            var payment = payments.Items[0];
            payment.UserId.ShouldBe(PrepaymentTestConsts.UserId);
            payment.PaymentMethod.ShouldBe(PrepaymentPaymentServiceProvider.PaymentMethod);
            payment.Currency.ShouldBe("CNY");
            payment.OriginalPaymentAmount.ShouldBe(150m);
            payment.ActualPaymentAmount.ShouldBe(150m);
            payment.RefundAmount.ShouldBe(decimal.Zero);
            payment.PendingRefundAmount.ShouldBe(decimal.Zero);
            payment.CanceledTime.ShouldBeNull();
            payment.CompletionTime.ShouldBeNull();
            payment.PaymentItems.Count.ShouldBe(2);
            
            var paymentItem1 = payment.PaymentItems.FirstOrDefault(x => x.ItemKey == itemKey1);
            paymentItem1.ShouldNotBeNull();
            paymentItem1.ItemType.ShouldBe("Test");
            paymentItem1.OriginalPaymentAmount.ShouldBe(50m);
            paymentItem1.ActualPaymentAmount.ShouldBe(50m);
            paymentItem1.PaymentDiscount.ShouldBe(decimal.Zero);
            paymentItem1.RefundAmount.ShouldBe(decimal.Zero);
            paymentItem1.PendingRefundAmount.ShouldBe(decimal.Zero);
            
            var paymentItem2 = payment.PaymentItems.FirstOrDefault(x => x.ItemKey == itemKey2);
            paymentItem2.ShouldNotBeNull();
            paymentItem2.ItemType.ShouldBe("Test");
            paymentItem2.OriginalPaymentAmount.ShouldBe(100m);
            paymentItem2.ActualPaymentAmount.ShouldBe(100m);
            paymentItem2.PaymentDiscount.ShouldBe(decimal.Zero);
            paymentItem2.RefundAmount.ShouldBe(decimal.Zero);
            paymentItem2.PendingRefundAmount.ShouldBe(decimal.Zero);

            return payment.Id;
        }

        [Fact]
        public async Task<PaymentDto> Should_Complete_Payment()
        {
            var id = await Should_Create_Payment();

            var input = new PayInput();
            
            input.SetProperty(PrepaymentConsts.PaymentAccountIdPropertyName, PrepaymentTestConsts.AccountId);

            var account = await _accountRepository.GetAsync(PrepaymentTestConsts.AccountId);
            account.Balance.ShouldBeGreaterThanOrEqualTo(150m);

            var originalBalance = account.Balance;

            var payment = await _paymentAppService.PayAsync(id, input);
            
            account = await _accountRepository.GetAsync(PrepaymentTestConsts.AccountId);
            account.Balance.ShouldNotBe(originalBalance);
            account.Balance.ShouldBe(originalBalance - payment.ActualPaymentAmount);

            payment.CompletionTime.ShouldNotBeNull();

            return payment;
        }

        [Fact]
        public async Task<PaymentDto> Should_Refund_A_Part()
        {
            var payment = await Should_Complete_Payment();
            payment.RefundAmount.ShouldBe(decimal.Zero);
            payment.PendingRefundAmount.ShouldBe(decimal.Zero);

            var item1 = payment.PaymentItems.FirstOrDefault();
            item1.ShouldNotBeNull();
            item1.RefundAmount.ShouldBe(decimal.Zero);
            item1.PendingRefundAmount.ShouldBe(decimal.Zero);

            await _distributedEventBus.PublishAsync(new RefundPaymentEto(null, new CreateRefundInput
                {
                    PaymentId = payment.Id,
                    DisplayReason = "Test0",
                    CustomerRemark = "Test1",
                    StaffRemark = "Test2",
                    RefundItems = new List<CreateRefundItemInput>
                    {
                        new()
                        {
                            PaymentItemId = item1.Id,
                            RefundAmount = 10m,
                            CustomerRemark = "Test3",
                            StaffRemark = "Test4",
                        }
                    }
                }
            ), false, false);
            
            payment = await _paymentAppService.GetAsync(payment.Id);
            
            payment.RefundAmount.ShouldBe(10m);
            payment.PendingRefundAmount.ShouldBe(decimal.Zero);
            item1 = payment.PaymentItems.FirstOrDefault(x => x.Id == item1.Id);
            item1.ShouldNotBeNull();
            item1.RefundAmount.ShouldBe(10m);
            item1.PendingRefundAmount.ShouldBe(decimal.Zero);

            var refunds = await _refundAppService.GetListAsync(new GetRefundListInput
            {
                PaymentId = payment.Id
            });

            var refund = refunds.Items.FirstOrDefault(x => x.PaymentId == payment.Id);
            refund.ShouldNotBeNull();
            refund.Currency.ShouldBe(payment.Currency);
            refund.CanceledTime.ShouldBeNull();
            refund.CompletedTime.ShouldNotBeNull();
            refund.DisplayReason.ShouldBe("Test0");
            refund.CustomerRemark.ShouldBe("Test1");
            refund.StaffRemark.ShouldBe("Test2");
            refund.RefundAmount.ShouldBe(10m);
            refund.RefundPaymentMethod.ShouldBe(PrepaymentPaymentServiceProvider.PaymentMethod);
            refund.ExternalTradingCode.ShouldBeNull();

            var refundItem = refund.RefundItems.FirstOrDefault(x => x.PaymentItemId == item1.Id);
            refundItem.ShouldNotBeNull();
            refundItem.CustomerRemark.ShouldBe("Test3");
            refundItem.StaffRemark.ShouldBe("Test4");
            refundItem.RefundAmount.ShouldBe(10m);

            var transactions = await _transactionRepository.GetListAsync();

            transactions.Count.ShouldBe(2);
            var paymentTransaction = transactions.Find(x => x.TransactionType == TransactionType.Credit);
            paymentTransaction.ShouldNotBeNull();
            paymentTransaction.Currency.ShouldBe(payment.Currency);
            paymentTransaction.AccountId.ShouldBe(PrepaymentTestConsts.AccountId);
            paymentTransaction.ActionName.ShouldBe(PrepaymentConsts.PaymentActionName);
            paymentTransaction.ChangedBalance.ShouldBe(-150m);
            paymentTransaction.OriginalBalance.ShouldBe(PrepaymentTestConsts.AccountBaseBalance);
            paymentTransaction.PaymentId.ShouldBe(payment.Id);
            paymentTransaction.PaymentMethod.ShouldBe(PrepaymentPaymentServiceProvider.PaymentMethod);
            paymentTransaction.TransactionType.ShouldBe(TransactionType.Credit);
            paymentTransaction.AccountUserId.ShouldBe(PrepaymentTestConsts.UserId);
            paymentTransaction.ExternalTradingCode.ShouldBeNull();
            
            var refundTransaction = transactions.Find(x => x.TransactionType == TransactionType.Debit);
            refundTransaction.ShouldNotBeNull();
            refundTransaction.Currency.ShouldBe(payment.Currency);
            refundTransaction.AccountId.ShouldBe(PrepaymentTestConsts.AccountId);
            refundTransaction.ActionName.ShouldBe(PrepaymentConsts.RefundActionName);
            refundTransaction.ChangedBalance.ShouldBe(+10m);
            refundTransaction.OriginalBalance.ShouldBe(PrepaymentTestConsts.AccountBaseBalance + paymentTransaction.ChangedBalance);
            refundTransaction.PaymentId.ShouldBe(payment.Id);
            refundTransaction.PaymentMethod.ShouldBe(PrepaymentPaymentServiceProvider.PaymentMethod);
            refundTransaction.TransactionType.ShouldBe(TransactionType.Debit);
            refundTransaction.AccountUserId.ShouldBe(PrepaymentTestConsts.UserId);
            refundTransaction.ExternalTradingCode.ShouldBeNull();

            return payment;
        }

        [Fact]
        public async Task Should_Refund_The_Rest()
        {
            var payment = await Should_Refund_A_Part();

            var item1 = payment.PaymentItems.FirstOrDefault(x => x.RefundAmount > decimal.Zero);
            item1.ShouldNotBeNull();
            var item2 = payment.PaymentItems.FirstOrDefault(x => x.RefundAmount == decimal.Zero);
            item2.ShouldNotBeNull();

            await _distributedEventBus.PublishAsync(new RefundPaymentEto(null, new CreateRefundInput
                {
                    PaymentId = payment.Id,
                    DisplayReason = "Test0",
                    CustomerRemark = "Test1",
                    StaffRemark = "Test2",
                    RefundItems = new List<CreateRefundItemInput>
                    {
                        new()
                        {
                            PaymentItemId = item1.Id,
                            RefundAmount = item1.ActualPaymentAmount - item1.RefundAmount,
                            CustomerRemark = "Test3",
                            StaffRemark = "Test4",
                        },
                        new()
                        {
                            PaymentItemId = item2.Id,
                            RefundAmount = item2.ActualPaymentAmount,
                            CustomerRemark = "Test5",
                            StaffRemark = "Test6",
                        }
                    }
                }
            ), false, false);
            
            payment = await _paymentAppService.GetAsync(payment.Id);
            
            payment.RefundAmount.ShouldBe(payment.ActualPaymentAmount);
            payment.PendingRefundAmount.ShouldBe(decimal.Zero);
            payment.PaymentItems.Sum(x => x.RefundAmount).ShouldBe(payment.RefundAmount);
        }

        [Fact]
        public async Task Should_Refund_More_Than_The_Actual_Payment_Amount()
        {
            var payment = await Should_Refund_A_Part();

            var item1 = payment.PaymentItems.FirstOrDefault(x => x.RefundAmount > decimal.Zero);
            item1.ShouldNotBeNull();

            await Assert.ThrowsAsync<InvalidRefundAmountException>(async () =>
            {
                await _distributedEventBus.PublishAsync(new RefundPaymentEto(null, new CreateRefundInput
                    {
                        PaymentId = payment.Id,
                        DisplayReason = "Test0",
                        CustomerRemark = "Test1",
                        StaffRemark = "Test2",
                        RefundItems = new List<CreateRefundItemInput>
                        {
                            new()
                            {
                                PaymentItemId = item1.Id,
                                RefundAmount = item1.ActualPaymentAmount - item1.RefundAmount + 0.01m,
                                CustomerRemark = "Test3",
                                StaffRemark = "Test4",
                            }
                        }
                    }
                ), false, false);
            });
        }
    }
}
