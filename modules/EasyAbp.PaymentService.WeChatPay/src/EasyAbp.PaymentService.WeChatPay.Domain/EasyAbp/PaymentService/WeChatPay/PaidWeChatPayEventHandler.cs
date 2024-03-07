using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Common.RequestHandling;
using EasyAbp.Abp.WeChat.Pay.RequestHandling;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.WeChatPay.Background;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class PaidWeChatPayEventHandler : IWeChatPayEventHandler, ITransientDependency
    {
        public WeChatHandlerType Type => WeChatHandlerType.Paid;

        private readonly IDataFilter _dataFilter;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaidWeChatPayEventHandler(
            IDataFilter dataFilter,
            IServiceScopeFactory serviceScopeFactory,
            IUnitOfWorkManager unitOfWorkManager,
            IBackgroundJobManager backgroundJobManager,
            IPaymentManager paymentManager,
            IPaymentRecordRepository paymentRecordRepository,
            IPaymentRepository paymentRepository)
        {
            _dataFilter = dataFilter;
            _serviceScopeFactory = serviceScopeFactory;
            _unitOfWorkManager = unitOfWorkManager;
            _backgroundJobManager = backgroundJobManager;
            _paymentManager = paymentManager;
            _paymentRecordRepository = paymentRecordRepository;
            _paymentRepository = paymentRepository;
        }

        [UnitOfWork(true)]
        public virtual async Task<WeChatRequestHandlingResult> HandleAsync(WeChatPayEventModel model)
        {
            var dict = model.WeChatRequestXmlData.SelectSingleNode("xml").ToDictionary() ??
                       throw new NullReferenceException();

            var returnCode = dict.GetOrDefault("return_code");
            var deviceInfo = dict.GetOrDefault("device_info");

            if (returnCode != "SUCCESS")
            {
                return new WeChatRequestHandlingResult(false, $"Unexpected return_code:{returnCode}");
            }

            if (deviceInfo != PaymentServiceWeChatPayConsts.DeviceInfo)
            {
                // skip handling
                return new WeChatRequestHandlingResult(true);
            }

            using var disabledDataFilter = _dataFilter.Disable<IMultiTenant>();

            var paymentId = Guid.Parse(dict.GetOrDefault("out_trade_no") ??
                                       throw new XmlDocumentMissingRequiredElementException("out_trade_no"));

            await RecordPaymentResultAsync(dict, paymentId);

            var payment = await _paymentRepository.GetAsync(paymentId);

            if (payment.IsInProgress())
            {
                payment.SetExternalTradingCode(dict.GetOrDefault("transaction_id") ??
                                               throw new XmlDocumentMissingRequiredElementException("transaction_id"));

                await _paymentRepository.UpdateAsync(payment, true);

                if (dict.GetOrDefault("result_code") == "SUCCESS")
                {
                    await _paymentManager.CompletePaymentAsync(payment);
                }
                else
                {
                    await _paymentManager.StartCancelAsync(payment);
                }
            }
            else if (payment.IsCanceled())
            {
                var outRefundNo = payment.Id.ToString();
                var args = new WeChatPayRefundJobArgs(payment.TenantId, payment.Id, outRefundNo,
                    payment.ActualPaymentAmount, PaymentServiceWeChatPayConsts.InvalidPaymentAutoRefundDisplayReason);

                // Refund the invalid payment.
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

            return new WeChatRequestHandlingResult(true);
        }

        protected virtual async Task<PaymentRecord> RecordPaymentResultAsync(Dictionary<string, string> dict,
            Guid paymentId)
        {
            var couponCount = ConvertToNullableInt32(dict.GetOrDefault("coupon_count"));

            var record = await _paymentRecordRepository.GetByPaymentId(paymentId);

            record.SetResult(
                returnCode: dict.GetOrDefault("return_code"),
                returnMsg: dict.GetOrDefault("return_msg"),
                appId: dict.GetOrDefault("appid"),
                subAppId: dict.GetOrDefault("sub_appid"),
                mchId: dict.GetOrDefault("mch_id"),
                subMchId: dict.GetOrDefault("sub_mch_id"),
                deviceInfo: dict.GetOrDefault("device_info"),
                resultCode: dict.GetOrDefault("result_code"),
                errCode: dict.GetOrDefault("err_code"),
                errCodeDes: dict.GetOrDefault("err_code_des"),
                openid: dict.GetOrDefault("openid"),
                isSubscribe: dict.GetOrDefault("is_subscribe"),
                tradeType: dict.GetOrDefault("trade_type"),
                bankType: dict.GetOrDefault("bank_type"),
                totalFee: ConvertToInt32(dict.GetOrDefault("total_fee")),
                settlementTotalFee: ConvertToNullableInt32(dict.GetOrDefault("total_fee")),
                feeType: dict.GetOrDefault("fee_type"),
                cashFee: ConvertToInt32(dict.GetOrDefault("cash_fee")),
                cashFeeType: dict.GetOrDefault("cash_fee_type"),
                couponFee: ConvertToNullableInt32(dict.GetOrDefault("coupon_fee")),
                couponCount: couponCount,
                couponTypes: couponCount != null
                    ? dict.JoinNodesInnerTextAsString("coupon_type_", couponCount.Value)
                    : null,
                couponIds: couponCount != null
                    ? dict.JoinNodesInnerTextAsString("coupon_id_", couponCount.Value)
                    : null,
                couponFees: couponCount != null
                    ? dict.JoinNodesInnerTextAsString("coupon_fee_", couponCount.Value)
                    : null,
                transactionId: dict.GetOrDefault("transaction_id"),
                outTradeNo: dict.GetOrDefault("out_trade_no"),
                attach: dict.GetOrDefault("attach"),
                timeEnd: dict.GetOrDefault("time_end")
            );

            return await _paymentRecordRepository.UpdateAsync(record, true);
        }

        private static int? ConvertToNullableInt32(string text)
        {
            if (text.IsNullOrEmpty())
            {
                return null;
            }

            return Convert.ToInt32(text);
        }

        private static int ConvertToInt32(string text)
        {
            return Convert.ToInt32(text);
        }
    }
}