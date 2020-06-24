using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using EasyAbp.Abp.WeChat.Pay.Infrastructure;
using EasyAbp.PaymentService.Payments;
using EasyAbp.PaymentService.Refunds;
using EasyAbp.PaymentService.WeChatPay.RefundRecords;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace EasyAbp.PaymentService.WeChatPay
{
    public class PaymentServiceWeChatPayRefundHandler : IWeChatPayRefundHandler, ITransientDependency
    {
        private readonly IDataFilter _dataFilter;
        private readonly IPaymentManager _paymentManager;
        private readonly IRefundRecordRepository _refundRecordRepository;
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentServiceWeChatPayRefundHandler(
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
        
        public virtual async Task HandleAsync(XmlDocument xmlDocument)
        {
            var dict = xmlDocument.SelectSingleNode("xml").ToDictionary() ?? throw new NullReferenceException();

            if (dict.GetOrDefault("return_code") != "SUCCESS")
            {
                return;
            }
            
            using var disabledDataFilter = _dataFilter.Disable<IMultiTenant>();

            var record = await _refundRecordRepository.FindAsync(x => x.Id == Guid.Parse(dict.GetOrDefault("out_refund_no")));

            if (record == null)
            {
                return;
            }
            
            var payment = await _paymentRepository.FindAsync(record.PaymentId);
            var refunds = await _refundRepository.GetOngoingRefundListAsync(record.PaymentId);

            if (payment == null || refunds.IsNullOrEmpty())
            {
                return;
            }
            
            record.SetInformationInNotify(
                refundStatus: dict.GetOrDefault("refund_status"),
                successTime: dict.GetOrDefault("success_time"),
                refundRecvAccout: dict.GetOrDefault("refund_recv_accout"),
                refundAccount: dict.GetOrDefault("refund_account"),
                refundRequestSource: dict.GetOrDefault("refund_request_source"),
                settlementRefundFee: Convert.ToInt32(dict.GetOrDefault("settlement_refund_fee")));

            await _refundRecordRepository.UpdateAsync(record, true);

            if (dict.GetOrDefault("refund_status") == "SUCCESS")
            {
                await _paymentManager.CompleteRefundAsync(payment, refunds);
            }
            else
            {
                await _paymentManager.RollbackRefundAsync(payment, refunds);
            }
        }
    }
}