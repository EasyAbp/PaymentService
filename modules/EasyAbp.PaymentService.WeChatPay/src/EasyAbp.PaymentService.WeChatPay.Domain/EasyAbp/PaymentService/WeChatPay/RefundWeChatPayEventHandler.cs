using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Common.RequestHandling;
using EasyAbp.Abp.WeChat.Pay.RequestHandling;
using EasyAbp.Abp.WeChat.Pay.RequestHandling.Models;
using EasyAbp.Abp.WeChat.Pay.Services.BasicPayment.Models;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class RefundWeChatPayEventHandler : WeChatPayRefundEventHandlerBase, ITransientDependency
    {
        private readonly IDataFilter _dataFilter;
        private readonly IPaymentManager _paymentManager;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IRefundRecordRepository _refundRecordRepository;
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentRepository _paymentRepository;

        public RefundWeChatPayEventHandler(
            IDataFilter dataFilter,
            IPaymentManager paymentManager,
            IJsonSerializer jsonSerializer,
            IRefundRecordRepository refundRecordRepository,
            IRefundRepository refundRepository,
            IPaymentRepository paymentRepository)
        {
            _dataFilter = dataFilter;
            _paymentManager = paymentManager;
            _jsonSerializer = jsonSerializer;
            _refundRecordRepository = refundRecordRepository;
            _refundRepository = refundRepository;
            _paymentRepository = paymentRepository;
        }

        [UnitOfWork(true)]
        public override async Task<WeChatRequestHandlingResult> HandleAsync(
            WeChatPayEventModel<WeChatPayRefundEventModel> model)
        {
            // todo: don't use _dataFilter.Disable<IMultiTenant>();
            using var disabledDataFilter = _dataFilter.Disable<IMultiTenant>();

            var record = await _refundRecordRepository.FindByOutRefundNoAsync(model.Resource.OutRefundNo);

            if (record is null)
            {
                // skip handling
                return new WeChatRequestHandlingResult(true);
            }

            var payment = await _paymentRepository.FindAsync(record.PaymentId);

            var refund = await _refundRepository.FindByPaymentIdAsync(record.PaymentId);

            if (payment is null)
            {
                return new WeChatRequestHandlingResult(false, $"Payment entity not found, id: {record.PaymentId}");
            }

            if (record.Status == "PROCESSING")
            {
                record.SetInformationInNotify(
                    status: model.Resource.RefundStatus,
                    successTime: model.Resource.SuccessTime?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    userReceivedAccount: model.Resource.UserReceivedAccount,
                    amount: _jsonSerializer.Serialize(model.Resource.Amount)
                );

                await _refundRecordRepository.UpdateAsync(record, true);
            }

            if (refund is not null)
            {
                if (model.Resource.RefundStatus == "SUCCESS")
                {
                    await HandleSuccessfulRefundAsync(payment, refund);
                }
                else
                {
                    await HandleFailedRefundAsync(payment, refund);
                }
            }

            return new WeChatRequestHandlingResult(true);
        }

        protected virtual async Task HandleSuccessfulRefundAsync(Payment payment, Refund refund)
        {
            await _paymentManager.CompleteRefundAsync(payment, refund);
        }

        protected virtual async Task HandleFailedRefundAsync(Payment payment, Refund refund)
        {
            await _paymentManager.RollbackRefundAsync(payment, refund);
        }
    }
}