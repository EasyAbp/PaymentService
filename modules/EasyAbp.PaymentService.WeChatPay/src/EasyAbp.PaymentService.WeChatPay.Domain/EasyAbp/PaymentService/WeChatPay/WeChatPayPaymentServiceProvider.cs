using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay.Options;
using EasyAbp.Abp.WeChat.Pay.Services;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment.AppPayment;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment.Enums;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment.H5Payment;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment.JSPayment;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment.Models;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment.NativePayment;
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
using CreateOrderRequest = EasyAbp.Abp.WeChat.Pay.Services.BasicPayment.JSPayment.Models.CreateOrderRequest;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class WeChatPayPaymentServiceProvider : PaymentServiceProvider
    {
        private readonly ISettingProvider _settingProvider;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IWeChatPayFeeConverter _weChatPayFeeConverter;
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IPaymentOpenIdProvider _paymentOpenIdProvider;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAbpWeChatPayOptionsProvider _abpWeChatPayOptionsProvider;
        private readonly IAbpWeChatPayServiceFactory _abpWeChatPayServiceFactory;

        public const string PaymentMethod = "WeChatPay";

        public WeChatPayPaymentServiceProvider(
            ISettingProvider settingProvider,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            IBackgroundJobManager backgroundJobManager,
            IWeChatPayFeeConverter weChatPayFeeConverter,
            IPaymentManager paymentManager,
            IPaymentRecordRepository paymentRecordRepository,
            IPaymentOpenIdProvider paymentOpenIdProvider,
            IPaymentRepository paymentRepository,
            IServiceScopeFactory serviceScopeFactory,
            IAbpWeChatPayOptionsProvider abpWeChatPayOptionsProvider,
            IAbpWeChatPayServiceFactory abpWeChatPayServiceFactory)
        {
            _settingProvider = settingProvider;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _unitOfWorkManager = unitOfWorkManager;
            _backgroundJobManager = backgroundJobManager;
            _weChatPayFeeConverter = weChatPayFeeConverter;
            _paymentManager = paymentManager;
            _paymentRecordRepository = paymentRecordRepository;
            _paymentOpenIdProvider = paymentOpenIdProvider;
            _paymentRepository = paymentRepository;
            _serviceScopeFactory = serviceScopeFactory;
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
                        Check.NotNullOrWhiteSpace(await _settingProvider.GetOrNullAsync(
                            AbpWeChatPaySettings.MchId), "mchId");

            Check.NotNullOrWhiteSpace(mchId, "mch_id");

            var tradeType = configurations.GetOrDefault("trade_type") as string;

            var options = await _abpWeChatPayOptionsProvider.GetAsync(mchId);

            var appId = configurations.GetOrDefault("appid") as string;

            var outTradeNo = payment.Id.ToString("N");

            var notifyUrl = configurations.GetOrDefault("notify_url") as string ?? options.NotifyUrl;

            var description = configurations.GetOrDefault("description") as string ??
                              PaymentServiceWeChatPayConsts.DefaultDescriptionOnPaymentCreation;

            switch (tradeType)
            {
                case TradeTypeEnum.JsApi:
                    await CreateJsApiOrderAsync(payment, appId, mchId, description, notifyUrl, outTradeNo);
                    break;
                case TradeTypeEnum.App:
                    await CreateAppOrderAsync(payment, appId, mchId, description, notifyUrl, outTradeNo);
                    break;
                case TradeTypeEnum.Native:
                    await CreateNativeOrderAsync(payment, appId, mchId, description, notifyUrl, outTradeNo);
                    break;
                case TradeTypeEnum.MWeb:
                    await CreateMWebOrderAsync(payment, appId, mchId, description, notifyUrl, outTradeNo);
                    break;
                default:
                    throw new UnsupportedWeChatPayTradeTypeException(tradeType);
            }

            payment.SetPayeeAccount(mchId);

            await _paymentRecordRepository.InsertAsync(
                new PaymentRecord(_guidGenerator.Create(), _currentTenant.Id, payment.Id), true);

            await _paymentRepository.UpdateAsync(payment, true);
        }

        protected virtual async Task CreateJsApiOrderAsync(Payment payment, string appId, string mchId,
            string description, string notifyUrl, string outTradeNo)
        {
            var openId = await _paymentOpenIdProvider.FindUserOpenIdAsync(appId, payment.UserId);

            var jsPaymentService =
                await _abpWeChatPayServiceFactory.CreateAsync<JsPaymentService>(mchId);

            var response = await jsPaymentService.CreateOrderAsync(new CreateOrderRequest
            {
                AppId = appId,
                MchId = mchId,
                Description = description,
                OutTradeNo = outTradeNo,
                Attach = PaymentServiceWeChatPayConsts.Attach,
                NotifyUrl = notifyUrl,
                Amount = new CreateOrderAmountModel
                {
                    Total = _weChatPayFeeConverter.ConvertToWeChatPayFee(payment.ActualPaymentAmount),
                    Currency = payment.Currency
                },
                Payer = new CreateOrderRequest.CreateOrderPayerModel
                {
                    OpenId = openId
                }
            });

            if (!response.Code.IsNullOrEmpty())
            {
                throw new UserFriendlyException($"微信支付下单失败，错误信息：[{response.Code}] {response.Message}");
            }

            payment.SetProperty("appid", appId);
            payment.SetProperty("trade_type", TradeTypeEnum.JsApi);
            payment.SetProperty("prepay_id", response.PrepayId);
        }

        protected virtual async Task CreateAppOrderAsync(Payment payment, string appId, string mchId,
            string description, string notifyUrl, string outTradeNo)
        {
            var appPaymentService =
                await _abpWeChatPayServiceFactory.CreateAsync<AppPaymentService>(mchId);

            var response = await appPaymentService.CreateOrderAsync(new CreateOrderRequest
            {
                AppId = appId,
                MchId = mchId,
                Description = description,
                OutTradeNo = outTradeNo,
                Attach = PaymentServiceWeChatPayConsts.Attach,
                NotifyUrl = notifyUrl,
                Amount = new CreateOrderAmountModel
                {
                    Total = _weChatPayFeeConverter.ConvertToWeChatPayFee(payment.ActualPaymentAmount),
                    Currency = payment.Currency
                }
            });

            if (!response.Code.IsNullOrEmpty())
            {
                throw new UserFriendlyException($"微信支付下单失败，错误信息：[{response.Code}] {response.Message}");
            }

            payment.SetProperty("appid", appId);
            payment.SetProperty("trade_type", TradeTypeEnum.App);
            payment.SetProperty("prepay_id", response.PrepayId);
        }

        protected virtual async Task CreateNativeOrderAsync(Payment payment, string appId, string mchId,
            string description, string notifyUrl, string outTradeNo)
        {
            var nativePaymentService =
                await _abpWeChatPayServiceFactory.CreateAsync<NativePaymentService>(mchId);

            var response = await nativePaymentService.CreateOrderAsync(new CreateOrderRequest
            {
                AppId = appId,
                MchId = mchId,
                Description = description,
                OutTradeNo = outTradeNo,
                Attach = PaymentServiceWeChatPayConsts.Attach,
                NotifyUrl = notifyUrl,
                Amount = new CreateOrderAmountModel
                {
                    Total = _weChatPayFeeConverter.ConvertToWeChatPayFee(payment.ActualPaymentAmount),
                    Currency = payment.Currency
                }
            });

            if (!response.Code.IsNullOrEmpty())
            {
                throw new UserFriendlyException($"微信支付下单失败，错误信息：[{response.Code}] {response.Message}");
            }

            payment.SetProperty("appid", appId);
            payment.SetProperty("trade_type", TradeTypeEnum.Native);
            payment.SetProperty("code_url", response.CodeUrl);
        }

        protected virtual async Task CreateMWebOrderAsync(Payment payment, string appId, string mchId,
            string description,
            string notifyUrl, string outTradeNo)
        {
            var h5PaymentService =
                await _abpWeChatPayServiceFactory.CreateAsync<H5PaymentService>(mchId);

            var response = await h5PaymentService.CreateOrderAsync(new CreateOrderRequest
            {
                AppId = appId,
                MchId = mchId,
                Description = description,
                OutTradeNo = outTradeNo,
                Attach = PaymentServiceWeChatPayConsts.Attach,
                NotifyUrl = notifyUrl,
                Amount = new CreateOrderAmountModel
                {
                    Total = _weChatPayFeeConverter.ConvertToWeChatPayFee(payment.ActualPaymentAmount),
                    Currency = payment.Currency
                }
            });

            if (!response.Code.IsNullOrEmpty())
            {
                throw new UserFriendlyException($"微信支付下单失败，错误信息：[{response.Code}] {response.Message}");
            }

            payment.SetProperty("appid", appId);
            payment.SetProperty("trade_type", TradeTypeEnum.MWeb);
            payment.SetProperty("prepay_id", response.PrepayId);
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

                _unitOfWorkManager.Current.OnCompleted(async () =>
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var backgroundJobManager = scope.ServiceProvider.GetRequiredService<IBackgroundJobManager>();
                    await backgroundJobManager.EnqueueAsync(args);
                });
            }
            else
            {
                _unitOfWorkManager.Current.OnCompleted(async () =>
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var job = scope.ServiceProvider.GetRequiredService<CloseWeChatPayOrderJob>();
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

                _unitOfWorkManager.Current.OnCompleted(async () =>
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var backgroundJobManager = scope.ServiceProvider.GetRequiredService<IBackgroundJobManager>();
                    await backgroundJobManager.EnqueueAsync(args);
                });
            }
            else
            {
                _unitOfWorkManager.Current.OnCompleted(async () =>
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var job = scope.ServiceProvider.GetRequiredService<WeChatPayRefundJob>();
                    await job.ExecuteAsync(args);
                });
            }
        }
    }
}