using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay.Options;
using EasyAbp.Abp.WeChat.Pay.RequestHandling;
using EasyAbp.Abp.WeChat.Pay.RequestHandling.Models;
using EasyAbp.Abp.WeChat.Pay.Services;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment.Models;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Json;
using Xunit;

namespace EasyAbp.PaymentService.WeChatPay.WeChatCallbackHandlers;

public class PaidWeChatPayEventHandlerTests : WeChatPayDomainTestBase
{
    protected AbpWeChatPayOptions AbpWeChatPayOptions { get; set; }
    protected IPaymentRepository PaymentRepository { get; set; }
    protected IPaymentRecordRepository PaymentRecordRepository { get; set; }
    protected IRefundRecordRepository RefundRecordRepository { get; set; }
    protected PaidWeChatPayEventHandler PaidWeChatPayEventHandler { get; set; }
    protected IAbpWeChatPayServiceFactory AbpWeChatPayServiceFactory { get; set; }
    protected IJsonSerializer JsonSerializer { get; set; }

    public PaidWeChatPayEventHandlerTests()
    {
        AbpWeChatPayOptions = ServiceProvider.GetRequiredService<IOptions<AbpWeChatPayOptions>>().Value;
        PaidWeChatPayEventHandler = ServiceProvider.GetRequiredService<PaidWeChatPayEventHandler>();
        RefundRecordRepository = ServiceProvider.GetRequiredService<IRefundRecordRepository>();
        JsonSerializer = ServiceProvider.GetRequiredService<IJsonSerializer>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        PaymentRepository = Substitute.For<IPaymentRepository>();
        services.AddTransient(_ => PaymentRepository);

        PaymentRecordRepository = Substitute.For<IPaymentRecordRepository>();
        services.AddTransient(_ => PaymentRecordRepository);

        AbpWeChatPayServiceFactory = Substitute.For<IAbpWeChatPayServiceFactory>();
        services.AddTransient(_ => AbpWeChatPayServiceFactory);
    }

    [Fact]
    public async Task Should_Complete_Payment()
    {
        var payment = GetNewPayment();

        await HandleAsync(payment);

        await WithUnitOfWorkAsync(async () =>
        {
            payment.ActualPaymentAmount.ShouldBe(100m);
            payment.RefundAmount.ShouldBe(0m);
            payment.PendingRefundAmount.ShouldBe(0m);
            payment.IsCompleted().ShouldBeTrue();

            var refundRecord = await RefundRecordRepository.FirstOrDefaultAsync(x => x.PaymentId == payment.Id);
            refundRecord.ShouldBeNull();
        });
    }

    [Fact]
    public async Task Should_Refund_If_Payment_Is_Canceled()
    {
        var payment = GetNewPayment();
        payment.CancelPayment(DateTime.Now);

        await HandleAsync(payment);

        await WithUnitOfWorkAsync(async () =>
        {
            var refundRecord = await RefundRecordRepository.FirstOrDefaultAsync(x => x.PaymentId == payment.Id);
            refundRecord.ShouldNotBeNull();
            refundRecord.Amount.ShouldNotBeNull();
            var amount = JsonSerializer.Deserialize<RefundOrderResponse.AmountInfo>(refundRecord.Amount);
            amount.Refund.ShouldBe(10000);
            amount.Currency.ShouldBe("CNY");
        });
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

        payment.SetPayeeAccount("10000100");

        return payment;
    }

    protected virtual async Task HandleAsync(Payment payment)
    {
        PaymentRepository.GetAsync(payment.Id).Returns(_ => payment);

        var paymentRecord = new PaymentRecord(Guid.NewGuid(), null, payment.Id);

        PaymentRecordRepository.GetByPaymentId(payment.Id).Returns(_ => paymentRecord);

        var basicPaymentService =
            Substitute.For<BasicPaymentService>(AbpWeChatPayOptions,
                new AbpLazyServiceProvider(ServiceProvider));

        AbpWeChatPayServiceFactory.CreateAsync<BasicPaymentService>()
            .ReturnsForAnyArgs(_ => basicPaymentService);

        var amount = Convert.ToInt32(Math.Floor(payment.ActualPaymentAmount * 100));
        var response = new RefundOrderResponse
        {
            RefundId = "2008450740201411110000174436",
            OutRefundNo = payment.Id.ToString(),
            TransactionId = "1008450740201411110005820873",
            OutTradeNo = payment.Id.ToString("N"),
            Channel = "ORIGINAL",
            UserReceivedAccount = "招商银行信用卡0403",
            SuccessTime = DateTime.Now,
            CreateTime = DateTime.Now,
            Status = "SUCCESS",
            Amount = new RefundOrderResponse.AmountInfo
            {
                Total = amount,
                Refund = amount,
                PayerTotal = amount,
                PayerRefund = amount,
                SettlementRefund = amount,
                SettlementTotal = amount,
                DiscountRefund = 0,
                Currency = payment.Currency
            }
        };

        basicPaymentService
            .RefundAsync(Arg.Any<RefundOrderRequest>())
            .Returns(_ => response);

        await WithUnitOfWorkAsync(async () =>
        {
            (await PaidWeChatPayEventHandler.HandleAsync(new WeChatPayEventModel<WeChatPayPaidEventModel>
            {
                Options = AbpWeChatPayOptions,
                Resource = new WeChatPayPaidEventModel
                {
                    AppId = "wx2421b1c4370ec43b",
                    MchId = "10000100",
                    OutTradeNo = payment.Id.ToString("N"),
                    TransactionId = "1008450740201411110005820873",
                    TradeType = "JSAPI",
                    TradeState = "SUCCESS",
                    BankType = "CMC",
                    Attach = PaymentServiceWeChatPayConsts.Attach,
                    SuccessTime = DateTime.Now,
                    Payer = new WeChatPayPaidEventModel.QueryOrderPayerModel
                    {
                        OpenId = "oUpF8uMEb4qRXf22hE3X68TekukE"
                    },
                    Amount = new WeChatPayPaidEventModel.QueryOrderAmountModel
                    {
                        Total = amount,
                        PayerTotal = amount,
                        Currency = payment.Currency,
                        PayerCurrency = payment.Currency
                    }
                }
            })).Success.ShouldBeTrue();
        });
    }
}