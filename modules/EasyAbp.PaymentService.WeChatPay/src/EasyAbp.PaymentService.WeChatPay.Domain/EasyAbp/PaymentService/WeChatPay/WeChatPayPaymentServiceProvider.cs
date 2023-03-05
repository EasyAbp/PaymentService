using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay.Options;
using EasyAbp.Abp.WeChat.Pay.Services;
using EasyAbp.Abp.WeChat.Pay.Services.Pay;
using EasyAbp.Abp.WeChat.Pay.Settings;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.WeChatPay.Background;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class WeChatPayPaymentServiceProvider : PaymentServiceProvider
    {
        private readonly ISettingProvider _settingProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IWeChatPayFeeConverter _weChatPayFeeConverter;
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IPaymentOpenIdProvider _paymentOpenIdProvider;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAbpWeChatPayOptionsProvider _abpWeChatPayOptionsProvider;
        private readonly IAbpWeChatPayServiceFactory _abpWeChatPayServiceFactory;

        public const string PaymentMethod = "WeChatPay";

        public WeChatPayPaymentServiceProvider(
            ISettingProvider settingProvider,
            IServiceProvider serviceProvider,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            IBackgroundJobManager backgroundJobManager,
            IWeChatPayFeeConverter weChatPayFeeConverter,
            IPaymentManager paymentManager,
            IPaymentRecordRepository paymentRecordRepository,
            IPaymentOpenIdProvider paymentOpenIdProvider,
            IPaymentRepository paymentRepository,
            IAbpWeChatPayOptionsProvider abpWeChatPayOptionsProvider,
            IAbpWeChatPayServiceFactory abpWeChatPayServiceFactory)
        {
            _settingProvider = settingProvider;
            _serviceProvider = serviceProvider;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _unitOfWorkManager = unitOfWorkManager;
            _backgroundJobManager = backgroundJobManager;
            _weChatPayFeeConverter = weChatPayFeeConverter;
            _paymentManager = paymentManager;
            _paymentRecordRepository = paymentRecordRepository;
            _paymentOpenIdProvider = paymentOpenIdProvider;
            _paymentRepository = paymentRepository;
            _abpWeChatPayOptionsProvider = abpWeChatPayOptionsProvider;
            _abpWeChatPayServiceFactory = abpWeChatPayServiceFactory;
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

            var mchId = configurations.GetOrDefault("mch_id") as string ??
                        await _settingProvider.GetOrNullAsync(AbpWeChatPaySettings.MchId);

            Check.NotNullOrWhiteSpace(mchId, "mch_id");

            var options = await _abpWeChatPayOptionsProvider.GetAsync(mchId);

            payment.SetPayeeAccount(mchId);

            var appId = configurations.GetOrDefault("appid") as string;

            var openId = await _paymentOpenIdProvider.FindUserOpenIdAsync(appId, payment.UserId);

            var outTradeNo = payment.Id.ToString("N");

            var serviceProviderPayService =
                await _abpWeChatPayServiceFactory.CreateAsync<ServiceProviderPayWeService>(mchId);

            var result = await serviceProviderPayService.UnifiedOrderAsync(
                appId: appId,
                subAppId: null,
                mchId: mchId,
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
                notifyUrl: configurations.GetOrDefault("notify_url") as string ?? options.NotifyUrl,
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
                throw new UnifiedOrderFailedException(dict.GetOrDefault("return_code"),
                    dict.GetOrDefault("return_msg"));
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

            var args = new CloseWeChatPayOrderJobArgs(
                tenantId: payment.TenantId,
                paymentId: payment.Id,
                outTradeNo: payment.Id.ToString("N"),
                appId: payment.GetProperty<string>("appid"),
                mchId: payment.PayeeAccount);

            if (_backgroundJobManager.IsAvailable())
            {
                // Enqueue an empty job to ensure the background job worker is alive.
                await _backgroundJobManager.EnqueueAsync(new EmptyJobArgs(payment.TenantId));

                _unitOfWorkManager.Current.OnCompleted(async () => { await _backgroundJobManager.EnqueueAsync(args); });
            }
            else
            {
                _unitOfWorkManager.Current.OnCompleted(async () =>
                {
                    var job = _serviceProvider.GetRequiredService<CloseWeChatPayOrderJob>();

                    await job.ExecuteAsync(args);
                });
            }
        }

        [UnitOfWork]
        public override async Task OnRefundStartedAsync(Payment payment, Refund refund)
        {
            var args = new WeChatPayRefundJobArgs(payment.TenantId, payment.Id, refund.Id);

            if (_backgroundJobManager.IsAvailable())
            {
                // Enqueue an empty job to ensure the background job worker is alive.
                await _backgroundJobManager.EnqueueAsync(new EmptyJobArgs(payment.TenantId));

                _unitOfWorkManager.Current.OnCompleted(async () => { await _backgroundJobManager.EnqueueAsync(args); });
            }
            else
            {
                _unitOfWorkManager.Current.OnCompleted(async () =>
                {
                    var job = _serviceProvider.GetRequiredService<WeChatPayRefundJob>();

                    await job.ExecuteAsync(args);
                });
            }
        }
    }
}