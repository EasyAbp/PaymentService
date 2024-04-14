using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using EasyAbp.Abp.WeChat.Pay.Options;
using EasyAbp.Abp.WeChat.Pay.RequestHandling;
using EasyAbp.Abp.WeChat.Pay.RequestHandling.Models;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Volo.Abp.Data;
using Xunit;

namespace EasyAbp.PaymentService.WeChatPay.WeChatCallbackHandlers;

public class RefundWeChatPayEventHandlerTests : WeChatPayDomainTestBase
{
    protected AbpWeChatPayOptions AbpWeChatPayOptions { get; set; }
    protected IPaymentRepository PaymentRepository { get; set; }
    protected IRefundRepository RefundRepository { get; set; }
    protected IRefundRecordRepository RefundRecordRepository { get; set; }
    protected RefundWeChatPayEventHandler RefundWeChatPayEventHandler { get; set; }

    public RefundWeChatPayEventHandlerTests()
    {
        AbpWeChatPayOptions = ServiceProvider.GetRequiredService<IOptions<AbpWeChatPayOptions>>().Value;
        RefundWeChatPayEventHandler =
            ServiceProvider.GetRequiredService<RefundWeChatPayEventHandler>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        PaymentRepository = Substitute.For<IPaymentRepository>();
        services.AddTransient(_ => PaymentRepository);

        RefundRepository = Substitute.For<IRefundRepository>();
        services.AddTransient(_ => RefundRepository);

        RefundRecordRepository = Substitute.For<IRefundRecordRepository>();
        services.AddTransient(_ => RefundRecordRepository);
    }

    [Fact]
    public async Task Should_Complete_Refund()
    {
        var payment = GetNewPayment();
        var refund = GetNewRefund(payment);

        await HandleAsync(payment, refund);

        payment.ActualPaymentAmount.ShouldBe(100m);
        payment.RefundAmount.ShouldBe(30m);
        payment.PendingRefundAmount.ShouldBe(0m);

        refund.RefundAmount.ShouldBe(30m);
        refund.IsCompleted().ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Not_Throw_If_No_Refund_Entity()
    {
        var payment = GetNewPayment();

        await Should.NotThrowAsync(async () => await HandleAsync(payment, null));
    }

    protected static Payment GetNewPayment()
    {
        var payment = new Payment(Guid.NewGuid(), null, Guid.NewGuid(), WeChatPayPaymentServiceProvider.PaymentMethod,
            "CNY", 100m, new List<PaymentItem>
            {
                new(Guid.NewGuid(), "TestType", "TestKey", 40m),
                new(Guid.NewGuid(), "TestType", "TestKey", 60m)
            });

        payment.SetProperty("appid", "wx2421b1c4370ec43b");
        payment.SetProperty("trade_type", "JSAPI");
        payment.SetProperty("prepay_id", "123456");
        payment.SetProperty("code_url", "123456");

        payment.CompletePayment(DateTime.Now);

        return payment;
    }

    protected static Refund GetNewRefund(Payment payment)
    {
        var refund = new Refund(Guid.NewGuid(), null, payment.Id, payment.PaymentMethod, null, payment.Currency,
            30m, "no-reason", null, null, new List<RefundItem>
            {
                new(Guid.NewGuid(), payment.PaymentItems[0].Id, 10m, null, null),
                new(Guid.NewGuid(), payment.PaymentItems[1].Id, 20m, null, null)
            });

        payment.StartRefund(refund);

        return refund;
    }

    protected virtual async Task HandleAsync(Payment payment, [CanBeNull] Refund refund)
    {
        PaymentRepository.FindAsync(payment.Id).Returns(payment);

        if (refund is not null)
        {
            RefundRepository.FindByPaymentIdAsync(payment.Id).Returns(refund);
        }

        var outRefundNo = refund is null ? payment.Id.ToString() : refund.Id.ToString();
        var refundRecord = new RefundRecord(Guid.NewGuid(), null, payment.Id, null, null, "wx2421b1c4370ec43b",
            "10000100", null, payment.Id.ToString("N"), null, outRefundNo, 10000, null, 3000, null, "CNY", 0, null,
            null, null, null, null, null, null);
        RefundRecordRepository.FindByOutRefundNoAsync(outRefundNo).Returns(refundRecord);

        var refundAmount = refund?.RefundAmount ?? payment.ActualPaymentAmount;

        var paymentAmountInt = Convert.ToInt32(Math.Floor(payment.ActualPaymentAmount * 100));
        var refundAmountInt = Convert.ToInt32(Math.Floor(refundAmount * 100));

        await WithUnitOfWorkAsync(async () =>
        {
            (await RefundWeChatPayEventHandler.HandleAsync(new WeChatPayEventModel<WeChatPayRefundEventModel>
            {
                Options = AbpWeChatPayOptions,
                Resource = new WeChatPayRefundEventModel
                {
                    MchId = "1900000100",
                    OutTradeNo = payment.Id.ToString("N"),
                    TransactionId = "4200000215201811190261405420",
                    OutRefundNo = outRefundNo,
                    RefundId = "50000408942018111907145868882",
                    RefundStatus = "SUCCESS",
                    SuccessTime = DateTime.Now,
                    UserReceivedAccount = "招商银行信用卡0403",
                    CreateTime = DateTime.Now,
                    Amount = new WeChatPayRefundEventModel.AmountInfo
                    {
                        Total = paymentAmountInt,
                        Refund = refundAmountInt,
                        PayerTotal = paymentAmountInt,
                        PayerRefund = refundAmountInt
                    }
                }
            })).Success.ShouldBeTrue();
        });
    }
}