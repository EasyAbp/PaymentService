using System;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Common.RequestHandling;
using EasyAbp.Abp.WeChat.Pay.RequestHandling;
using EasyAbp.Abp.WeChat.Pay.RequestHandling.Models;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.WeChatPay.Background;
using EasyAbp.PaymentService.WeChatPay.PaymentRecords;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class PaidWeChatPayEventHandler : WeChatPayPaidEventHandlerBase, ITransientDependency
    {
        private readonly IDataFilter _dataFilter;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaidWeChatPayEventHandler(
            IDataFilter dataFilter,
            IJsonSerializer jsonSerializer,
            IServiceScopeFactory serviceScopeFactory,
            IUnitOfWorkManager unitOfWorkManager,
            IBackgroundJobManager backgroundJobManager,
            IPaymentManager paymentManager,
            IPaymentRecordRepository paymentRecordRepository,
            IPaymentRepository paymentRepository)
        {
            _dataFilter = dataFilter;
            _jsonSerializer = jsonSerializer;
            _serviceScopeFactory = serviceScopeFactory;
            _unitOfWorkManager = unitOfWorkManager;
            _backgroundJobManager = backgroundJobManager;
            _paymentManager = paymentManager;
            _paymentRecordRepository = paymentRecordRepository;
            _paymentRepository = paymentRepository;
        }

        [UnitOfWork(true)]
        public override async Task<WeChatRequestHandlingResult> HandleAsync(
            WeChatPayEventModel<WeChatPayPaidEventModel> model)
        {
            if (model.Resource.Attach != PaymentServiceWeChatPayConsts.Attach)
            {
                // skip handling
                return new WeChatRequestHandlingResult(true);
            }

            // todo: don't use _dataFilter.Disable<IMultiTenant>();
            using var disabledDataFilter = _dataFilter.Disable<IMultiTenant>();

            var paymentId = Guid.Parse(model.Resource.OutTradeNo);

            await RecordPaymentResultAsync(model.Resource, paymentId);

            var payment = await _paymentRepository.GetAsync(paymentId);

            if (payment.IsInProgress())
            {
                payment.SetExternalTradingCode(model.Resource.TransactionId);

                await _paymentRepository.UpdateAsync(payment, true);

                if (model.Resource.TradeState == "SUCCESS")
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

                    _unitOfWorkManager.Current!.OnCompleted(async () =>
                    {
                        using var scope = _serviceScopeFactory.CreateScope();
                        var backgroundJobManager = scope.ServiceProvider.GetRequiredService<IBackgroundJobManager>();
                        await backgroundJobManager.EnqueueAsync(args);
                    });
                }
                else
                {
                    _unitOfWorkManager.Current!.OnCompleted(async () =>
                    {
                        using var scope = _serviceScopeFactory.CreateScope();
                        var job = scope.ServiceProvider.GetRequiredService<WeChatPayRefundJob>();
                        await job.ExecuteAsync(args);
                    });
                }
            }

            return new WeChatRequestHandlingResult(true);
        }

        protected virtual async Task<PaymentRecord> RecordPaymentResultAsync(WeChatPayPaidEventModel model,
            Guid paymentId)
        {
            var record = await _paymentRecordRepository.GetByPaymentId(paymentId);

            record.SetResult(
                appId: model.AppId,
                mchId: model.MchId,
                outTradeNo: model.OutTradeNo,
                transactionId: model.TransactionId,
                tradeType: model.TradeType,
                tradeState: model.TradeState,
                tradeStateDesc: model.TradeStateDesc,
                bankType: model.BankType,
                attach: model.Attach,
                successTime: model.SuccessTime?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                payer: _jsonSerializer.Serialize(model.Payer),
                amount: _jsonSerializer.Serialize(model.Amount),
                sceneInfo: _jsonSerializer.Serialize(model.SceneInfo),
                promotionDetail: _jsonSerializer.Serialize(model.PromotionDetails)
            );

            return await _paymentRecordRepository.UpdateAsync(record, true);
        }
    }
}