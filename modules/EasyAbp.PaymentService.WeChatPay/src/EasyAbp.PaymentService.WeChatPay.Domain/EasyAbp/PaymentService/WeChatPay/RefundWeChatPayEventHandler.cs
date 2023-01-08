using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Common.RequestHandling;
using EasyAbp.Abp.WeChat.Pay.RequestHandling;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class RefundWeChatPayEventHandler : IWeChatPayEventHandler, ITransientDependency
    {
        public WeChatHandlerType Type => WeChatHandlerType.Refund;

        private readonly IDataFilter _dataFilter;
        private readonly IPaymentManager _paymentManager;
        private readonly IRefundRecordRepository _refundRecordRepository;
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentRepository _paymentRepository;

        public RefundWeChatPayEventHandler(
            IDataFilter dataFilter,
            IPaymentManager paymentManager,
            IRefundRecordRepository refundRecordRepository,
            IRefundRepository refundRepository,
            IPaymentRepository paymentRepository)
        {
            _dataFilter = dataFilter;
            _paymentManager = paymentManager;
            _refundRecordRepository = refundRecordRepository;
            _refundRepository = refundRepository;
            _paymentRepository = paymentRepository;
        }

        [UnitOfWork(true)]
        public virtual async Task<WeChatRequestHandlingResult> HandleAsync(WeChatPayEventModel model)
        {
            var outerDict = model.WeChatRequestXmlData.SelectSingleNode("xml").ToDictionary() ??
                       throw new NullReferenceException();

            var dict = model.DecryptedXmlData.SelectSingleNode("root").ToDictionary() ??
                       throw new NullReferenceException();

            var returnCode = outerDict.GetOrDefault("return_code");
            var outRefundNo = dict.GetOrDefault("out_refund_no");

            if (returnCode != "SUCCESS")
            {
                return new WeChatRequestHandlingResult(false, $"Unexpected return_code:{returnCode}");
            }

            using var disabledDataFilter = _dataFilter.Disable<IMultiTenant>();

            var record = await _refundRecordRepository.FindByOutRefundNoAsync(outRefundNo);

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

            record.SetInformationInNotify(
                refundStatus: dict.GetOrDefault("refund_status"),
                successTime: dict.GetOrDefault("success_time"),
                refundRecvAccout: dict.GetOrDefault("refund_recv_accout"),
                refundAccount: dict.GetOrDefault("refund_account"),
                refundRequestSource: dict.GetOrDefault("refund_request_source"),
                settlementRefundFee: Convert.ToInt32(dict.GetOrDefault("settlement_refund_fee")));

            await _refundRecordRepository.UpdateAsync(record, true);

            if (refund is not null)
            {
                if (dict.GetOrDefault("refund_status") == "SUCCESS")
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