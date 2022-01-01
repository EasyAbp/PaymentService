﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay.Services.Pay;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using EasyAbp.PaymentService.WeChatPay.Settings;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class WeChatPayPaymentServiceProvider : PaymentServiceProvider
    {
        private readonly ServiceProviderPayService _serviceProviderPayService;
        private readonly ISettingProvider _settingProvider;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly IDistributedEventBus _distributedEventBus;
        private readonly IWeChatPayFeeConverter _weChatPayFeeConverter;
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IPaymentOpenIdProvider _paymentOpenIdProvider;
        private readonly IPaymentRepository _paymentRepository;

        public const string PaymentMethod = "WeChatPay";
        
        public WeChatPayPaymentServiceProvider(
            ServiceProviderPayService serviceProviderPayService,
            ISettingProvider settingProvider,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            IDistributedEventBus distributedEventBus,
            IWeChatPayFeeConverter weChatPayFeeConverter,
            IPaymentManager paymentManager,
            IPaymentRecordRepository paymentRecordRepository,
            IPaymentOpenIdProvider paymentOpenIdProvider,
            IPaymentRepository paymentRepository)
        {
            _serviceProviderPayService = serviceProviderPayService;
            _settingProvider = settingProvider;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _distributedEventBus = distributedEventBus;
            _weChatPayFeeConverter = weChatPayFeeConverter;
            _paymentManager = paymentManager;
            _paymentRecordRepository = paymentRecordRepository;
            _paymentOpenIdProvider = paymentOpenIdProvider;
            _paymentRepository = paymentRepository;
        }

        [UnitOfWork(true)]
        public override async Task OnPaymentStartedAsync(Payment payment, ExtraPropertyDictionary configurations)
        {
            if (payment.Currency != "CNY")
            {
                throw new CurrencyNotSupportedException(payment.PaymentMethod, payment.Currency);
            }

            if (payment.ActualPaymentAmount <= decimal.Zero)
            {
                throw new PaymentAmountInvalidException(payment.ActualPaymentAmount, payment.PaymentMethod);
            }

            var payeeAccount = configurations.GetOrDefault("PayeeAccount") as string ??
                               await _settingProvider.GetOrNullAsync(WeChatPaySettings.MchId);

            Check.NotNullOrWhiteSpace(payeeAccount, "PayeeAccount");
            
            payment.SetPayeeAccount(payeeAccount);

            var appId = configurations.GetOrDefault("appid") as string;
            
            var openId = await _paymentOpenIdProvider.FindUserOpenIdAsync(appId, payment.UserId);
            
            var outTradeNo = payment.Id.ToString("N");

            var result = await _serviceProviderPayService.UnifiedOrderAsync(
                appId: appId,
                subAppId: null,
                mchId: payment.PayeeAccount,
                subMchId: null,
                deviceInfo: PaymentServiceWeChatPayConsts.DeviceInfo,
                body: configurations.GetOrDefault("body") as string ?? "EasyAbpPaymentService",
                detail: configurations.GetOrDefault("detail") as string,
                attach: configurations.GetOrDefault("attach") as string,
                outTradeNo: outTradeNo,
                feeType: payment.Currency,
                totalFee: _weChatPayFeeConverter.ConvertToWeChatPayFee(payment.ActualPaymentAmount),
                billCreateIp: "127.0.0.1",
                timeStart: null,
                timeExpire: null,
                goodsTag: configurations.GetOrDefault("goods_tag") as string,
                notifyUrl: configurations.GetOrDefault("notify_url") as string 
                           ?? await _settingProvider.GetOrNullAsync(WeChatPaySettings.NotifyUrl),
                tradeType: configurations.GetOrDefault("trade_type") as string,
                productId: null,
                limitPay: configurations.GetOrDefault("limit_pay") as string,
                openId: openId,
                subOpenId: null,
                receipt: configurations.GetOrDefault("receipt") as string ?? "N",
                sceneInfo: null);

            var dict = result.SelectSingleNode("xml").ToDictionary() ?? throw new NullReferenceException();

            if (dict.GetOrDefault("return_code") != "SUCCESS")
            {
                throw new UnifiedOrderFailedException(dict.GetOrDefault("return_code"), dict.GetOrDefault("return_msg"));
            }

            if (dict.GetOrDefault("result_code") != "SUCCESS")
            {
                throw new UnifiedOrderFailedException(
                    dict.GetOrDefault("return_code"),
                    dict.GetOrDefault("return_msg"),
                    dict.GetOrDefault("err_code_des"),
                    dict.GetOrDefault("err_code")
                );
            }

            payment.SetProperty("appid", configurations.GetOrDefault("appid") as string);
            
            payment.SetProperty("trade_type", dict.GetOrDefault("trade_type"));
            payment.SetProperty("prepay_id", dict.GetOrDefault("prepay_id"));
            payment.SetProperty("code_url", dict.GetOrDefault("code_url"));
            
            await _paymentRecordRepository.InsertAsync(
                new PaymentRecord(_guidGenerator.Create(), _currentTenant.Id, payment.Id), true);
            
            await _paymentRepository.UpdateAsync(payment, true);
        }

        [UnitOfWork(true)]
        public override async Task OnCancelStartedAsync(Payment payment)
        {
            await _paymentManager.CompleteCancelAsync(payment);

            if (payment.PayeeAccount == null)
            {
                return;
            }

            await _distributedEventBus.PublishAsync(new CloseWeChatPayOrderEto(
                tenantId: payment.TenantId,
                paymentId: payment.Id,
                outTradeNo: payment.Id.ToString("N"),
                appId: payment.GetProperty<string>("appid"),
                mchId: payment.PayeeAccount));
        }

        [UnitOfWork]
        public override async Task OnRefundStartedAsync(Payment payment, Refund refund)
        {
            await _distributedEventBus.PublishAsync(new WeChatPayRefundEto(payment.Id, refund));
        }
    }
}