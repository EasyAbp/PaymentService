using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Pay.Options;
using EasyAbp.Abp.WeChat.Pay.Services;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment.Models;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.WeChatPay.Background
{
    public class WeChatPayRefundJob : IAsyncBackgroundJob<WeChatPayRefundJobArgs>, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ILogger<WeChatPayRefundJob> _logger;
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IRefundRecordRepository _refundRecordRepository;
        private readonly IWeChatPayFeeConverter _weChatPayFeeConverter;
        private readonly IAbpWeChatPayOptionsProvider _abpWeChatPayOptionsProvider;
        private readonly IAbpWeChatPayServiceFactory _abpWeChatPayServiceFactory;

        public WeChatPayRefundJob(
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            IJsonSerializer jsonSerializer,
            ILogger<WeChatPayRefundJob> logger,
            IRefundRepository refundRepository,
            IPaymentManager paymentManager,
            IPaymentRepository paymentRepository,
            IPaymentRecordRepository paymentRecordRepository,
            IRefundRecordRepository refundRecordRepository,
            IWeChatPayFeeConverter weChatPayFeeConverter,
            IAbpWeChatPayOptionsProvider abpWeChatPayOptionsProvider,
            IAbpWeChatPayServiceFactory abpWeChatPayServiceFactory)
        {
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _jsonSerializer = jsonSerializer;
            _logger = logger;
            _refundRepository = refundRepository;
            _paymentManager = paymentManager;
            _paymentRepository = paymentRepository;
            _paymentRecordRepository = paymentRecordRepository;
            _refundRecordRepository = refundRecordRepository;
            _weChatPayFeeConverter = weChatPayFeeConverter;
            _abpWeChatPayOptionsProvider = abpWeChatPayOptionsProvider;
            _abpWeChatPayServiceFactory = abpWeChatPayServiceFactory;
        }

        [UnitOfWork(true)]
        public virtual async Task ExecuteAsync(WeChatPayRefundJobArgs args)
        {
            using var change = _currentTenant.Change(args.TenantId);

            var payment = await _paymentRepository.GetAsync(args.PaymentId);

            // Try to lock the row in DB.
            await _paymentRepository.UpdateAsync(payment, true);

            Refund refund = null;

            if (args.RefundId is not null)
            {
                refund = await _refundRepository.GetAsync(args.RefundId.Value);

                // Try to lock the row in DB.
                await _refundRepository.UpdateAsync(refund, true);

                if (payment.TenantId != refund.TenantId ||
                    payment.Id != refund.PaymentId ||
                    !refund.IsInProgress())
                {
                    throw new RefundIsInUnexpectedStageException(refund.Id);
                }

                args.RefundAmount = refund.RefundAmount;
                args.OutRefundNo = refund.Id.ToString();
                args.DisplayReason = refund.DisplayReason;
            }

            var paymentRecord = await _paymentRecordRepository.GetByPaymentId(payment.Id);

            RefundOrderResponse response;

            try
            {
                response = await RequestWeChatPayRefundAsync(
                    payment, paymentRecord, args.RefundAmount, args.OutRefundNo, args.DisplayReason);
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                throw;
            }

            if (refund is not null)
            {
                var externalTradingCode = response.RefundId;

                refund.SetExternalTradingCode(externalTradingCode);

                await _refundRepository.UpdateAsync(refund, true);

                if (response.Status != "SUCCESS" && response.Status != "PROCESSING")
                {
                    _logger.LogError("Failed to refund WeChatPay payment, response: {Response}",
                        _jsonSerializer.Serialize(response));

                    await _paymentManager.RollbackRefundAsync(payment, refund);
                }
            }
        }

        protected virtual async Task CreateWeChatPayRefundRecordEntitiesAsync(Payment payment,
            RefundOrderResponse response)
        {
            await _refundRecordRepository.InsertAsync(new RefundRecord(
                id: _guidGenerator.Create(),
                tenantId: _currentTenant.Id,
                paymentId: payment.Id,
                refundId: response.RefundId,
                outRefundNo: response.OutRefundNo,
                transactionId: response.TransactionId,
                outTradeNo: response.OutTradeNo,
                channel: response.Channel,
                userReceivedAccount: response.UserReceivedAccount,
                successTime: response.SuccessTime?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                createTime: response.CreateTime?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                status: response.Status,
                fundsAccount: response.FundsAccount,
                amount: _jsonSerializer.Serialize(response.Amount),
                promotionDetail: _jsonSerializer.Serialize(response.PromotionDetail)
            ), true);
        }

        private async Task<RefundOrderResponse> RequestWeChatPayRefundAsync(Payment payment,
            PaymentRecord paymentRecord, decimal refundAmount, [NotNull] string outRefundNo,
            [CanBeNull] string displayReason)
        {
            var mchId = payment.PayeeAccount;
            var options = await _abpWeChatPayOptionsProvider.GetAsync(mchId);

            var basicPaymentService = await _abpWeChatPayServiceFactory.CreateAsync<BasicPaymentService>(mchId);

            var response = await basicPaymentService.RefundAsync(new RefundOrderRequest
            {
                TransactionId = paymentRecord.TransactionId,
                OutTradeNo = payment.Id.ToString("N"),
                OutRefundNo = outRefundNo,
                Reason = displayReason,
                NotifyUrl = options.RefundNotifyUrl,
                Amount = new RefundOrderRequest.AmountInfo
                {
                    Refund = _weChatPayFeeConverter.ConvertToWeChatPayFee(refundAmount),
                    Total = paymentRecord.TotalFee,
                    Currency = paymentRecord.FeeType
                }
            });

            if (!response.Code.IsNullOrEmpty())
            {
                throw new AbpException($"微信支付自动退款失败，错误信息：[{response.Code}] {response.Message}");
            }

            await CreateWeChatPayRefundRecordEntitiesAsync(payment, response);

            return response;
        }
    }
}