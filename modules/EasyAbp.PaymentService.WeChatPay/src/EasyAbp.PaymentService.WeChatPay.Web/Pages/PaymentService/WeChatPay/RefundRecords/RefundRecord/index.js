$(function () {

    var l = abp.localization.getResource('EasyAbpPaymentServiceWeChatPay');

    var service = easyAbp.paymentService.weChatPay.refundRecords.refundRecord;

    var dataTable = $('#RefundRecordTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(service.getList),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l('Detail'),
                                action: function (data) {
                                }
                            }
                        ]
                }
            },
            { data: "paymentId" },
            { data: "outRefundNo" },
            { data: "transactionId" },
            { data: "outTradeNo" },
            { data: "successTime", dataFormat: 'datetime' },
            { data: "channel" },
            { data: "userReceivedAccount" },
            { data: "createTime", dataFormat: 'datetime' },
            { data: "status" },
            { data: "fundsAccount" },
            { data: "amount" },
            { data: "promotionDetail" },
            { data: "returnCode" },
            { data: "returnMsg" },
            { data: "appId" },
            { data: "mchId" },
            { data: "refundId" },
            { data: "totalFee" },
            { data: "settlementTotalFee" },
            { data: "refundFee" },
            { data: "settlementRefundFee" },
            { data: "feeType" },
            { data: "cashFee" },
            { data: "cashFeeType" },
            { data: "cashRefundFee" },
            { data: "couponRefundFee" },
            { data: "couponRefundCount" },
            { data: "couponTypes" },
            { data: "couponIds" },
            { data: "couponRefundFees" },
            { data: "refundStatus" },
            { data: "refundRecvAccout" },
            { data: "refundAccount" },
            { data: "refundRequestSource" },
        ]
    }));
});