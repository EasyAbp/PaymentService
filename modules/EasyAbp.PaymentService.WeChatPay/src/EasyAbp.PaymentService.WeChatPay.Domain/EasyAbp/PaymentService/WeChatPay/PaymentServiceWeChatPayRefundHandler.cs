using System;
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
        private readonly IClock _clock;
        private readonly IDataFilter _dataFilter;
        private readonly IRefundRecordRepository _refundRecordRepository;
        private readonly IRefundRepository _refundRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentServiceWeChatPayRefundHandler(
            IClock clock,
            IDataFilter dataFilter,
            IRefundRecordRepository refundRecordRepository,
            IRefundRepository refundRepository,
            IPaymentRepository paymentRepository)
        {
            _clock = clock;
            _dataFilter = dataFilter;
            _refundRecordRepository = refundRecordRepository;
            _refundRepository = refundRepository;
            _paymentRepository = paymentRepository;
        }
        
        public virtual async Task HandleAsync(XmlDocument xmlDocument)
        {
            using var disabledDataFilter = _dataFilter.Disable<IMultiTenant>();

            var reader = new XmlNodeReader(xmlDocument.SelectSingleNode("xml") ?? throw new NullReferenceException());

            if (reader["return_code"] != "SUCCESS")
            {
                return;
            }

            var record = await _refundRecordRepository.FindAsync(x => x.Id == Guid.Parse(reader["out_refund_no"]));

            if (record == null)
            {
                return;
            }
            
            var payment = await _paymentRepository.FindAsync(record.PaymentId);
            var refund = await _refundRepository.GetOngoingRefundOrNullAsync(record.PaymentId);

            if (payment == null || refund == null)
            {
                return;
            }

            if (reader["refund_status"] != "SUCCESS")
            {
                await HandleRefundFailureAsync(payment, refund);

                return;
            }
            
            await HandleRefundSuccessAsync(payment, refund, record, reader);
        }

        protected virtual async Task HandleRefundFailureAsync(Payment payment, Refund refund)
        {
            payment.RollbackOngoingRefund();
                
            await _paymentRepository.UpdateAsync(payment, true);
                
            refund.CancelRefund(_clock.Now);

            await _refundRepository.UpdateAsync(refund, true);
        }
        
        protected virtual async Task HandleRefundSuccessAsync(Payment payment, Refund refund, RefundRecord record, XmlNodeReader reader)
        {
            payment.CompleteOngoingRefund();
                
            await _paymentRepository.UpdateAsync(payment, true);
            
            refund.CompleteRefund(_clock.Now);

            await _paymentRepository.UpdateAsync(payment, true);

            record.SetInformationInNotify(
                refundStatus: reader["refund_status"],
                successTime: reader["success_time"],
                refundRecvAccout: reader["refund_recv_accout"],
                refundAccount: reader["refund_account"],
                refundRequestSource: reader["refund_request_source"],
                settlementRefundFee: Convert.ToInt32(reader["settlement_refund_fee"]));

            await _refundRecordRepository.UpdateAsync(record, true);
        }
    }
}