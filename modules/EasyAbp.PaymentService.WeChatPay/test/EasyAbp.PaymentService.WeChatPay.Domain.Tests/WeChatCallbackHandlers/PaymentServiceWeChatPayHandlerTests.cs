using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using EasyAbp.Abp.WeChat.Pay.Infrastructure.OptionResolve;
using EasyAbp.Abp.WeChat.Pay.Services.Pay;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.PaymentService.WeChatPay.WeChatCallbackHandlers;

public class PaymentServiceWeChatPayHandlerTests : WeChatPayDomainTestBase
{
    protected IPaymentRepository PaymentRepository { get; set; }
    protected IPaymentRecordRepository PaymentRecordRepository { get; set; }
    protected ServiceProviderPayService ServiceProviderPayService { get; set; }
    protected IRefundRecordRepository RefundRecordRepository { get; set; }
    protected PaymentServiceWeChatPayHandler PaymentServiceWeChatPayHandler { get; set; }

    public PaymentServiceWeChatPayHandlerTests()
    {
        PaymentServiceWeChatPayHandler = ServiceProvider.GetRequiredService<PaymentServiceWeChatPayHandler>();
        RefundRecordRepository = ServiceProvider.GetRequiredService<IRefundRecordRepository>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        PaymentRepository = Substitute.For<IPaymentRepository>();
        services.AddTransient(_ => PaymentRepository);

        PaymentRecordRepository = Substitute.For<IPaymentRecordRepository>();
        services.AddTransient(_ => PaymentRecordRepository);

        ServiceProviderPayService = Substitute.For<ServiceProviderPayService>();
        services.AddTransient(_ => ServiceProviderPayService);
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
            refundRecord.RefundFee.ShouldBe(10000);
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

        return payment;
    }

    protected virtual async Task HandleAsync(Payment payment)
    {
        PaymentRepository.GetAsync(payment.Id).Returns(payment);

        var paymentRecord = new PaymentRecord(Guid.NewGuid(), null, payment.Id);

        PaymentRecordRepository.GetByPaymentId(payment.Id).Returns(paymentRecord);

        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(@$"
<xml>
  <appid><![CDATA[wx2421b1c4370ec43b]]></appid>
  <attach><![CDATA[支付测试]]></attach>
  <bank_type><![CDATA[CFT]]></bank_type>
  <fee_type><![CDATA[CNY]]></fee_type>
  <is_subscribe><![CDATA[Y]]></is_subscribe>
  <mch_id><![CDATA[10000100]]></mch_id>
  <nonce_str><![CDATA[5d2b6c2a8db53831f7eda20af46e531c]]></nonce_str>
  <openid><![CDATA[oUpF8uMEb4qRXf22hE3X68TekukE]]></openid>
  <out_trade_no><![CDATA[{payment.Id:N}]]></out_trade_no>
  <result_code><![CDATA[SUCCESS]]></result_code>
  <return_code><![CDATA[SUCCESS]]></return_code>
  <sign><![CDATA[B552ED6B279343CB493C5DD0D78AB241]]></sign>
  <time_end><![CDATA[20140903131540]]></time_end>
  <total_fee>{Math.Floor(payment.ActualPaymentAmount * 100)}</total_fee>
  <trade_type><![CDATA[JSAPI]]></trade_type>
  <transaction_id><![CDATA[1004400740201409030005092168]]></transaction_id>
  <device_info><![CDATA[EasyAbpPaymentService]]></device_info>
</xml>
");

        var xmlDocumentRefund = new XmlDocument();
        xmlDocumentRefund.LoadXml(@$"
<xml>
   <return_code><![CDATA[SUCCESS]]></return_code>
   <return_msg><![CDATA[OK]]></return_msg>
   <appid><![CDATA[wx2421b1c4370ec43b]]></appid>
   <mch_id><![CDATA[10000100]]></mch_id>
   <nonce_str><![CDATA[NfsMFbUFpdbEhPXP]]></nonce_str>
   <sign><![CDATA[B7274EB9F8925EB93100DD2085FA56C0]]></sign>
   <result_code><![CDATA[SUCCESS]]></result_code>
   <transaction_id><![CDATA[1008450740201411110005820873]]></transaction_id>
  <out_refund_no><![CDATA[{payment.Id}]]></out_refund_no>
  <out_trade_no><![CDATA[{payment.Id:N}]]></out_trade_no>
   <refund_id><![CDATA[2008450740201411110000174436]]></refund_id>
   <refund_fee>{Math.Floor(payment.ActualPaymentAmount * 100)}</refund_fee>
</xml>
");

        ServiceProviderPayService
            .RefundAsync(null, null, null, null, null, null, null, 0, 0, null, null, null, null)
            .ReturnsForAnyArgs(_ => xmlDocumentRefund);

        await WithUnitOfWorkAsync(async () =>
        {
            var context = new WeChatPayHandlerContext
            {
                WeChatRequestXmlData = xmlDocument,
                IsSuccess = true
            };

            await PaymentServiceWeChatPayHandler.HandleAsync(context);
            context.IsSuccess.ShouldBeTrue();
        });
    }
}