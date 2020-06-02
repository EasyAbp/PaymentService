$(function () {

    var l = abp.localization.getResource('WeChatPay');

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
            { data: "tenantId" },
            { data: "paymentId" },
            { data: "returnCode" },
            { data: "returnMsg" },
            { data: "appId" },
            { data: "mchId" },
            { data: "nonceStr" },
            { data: "reqInfo" },
            { data: "transactionId" },
            { data: "outTradeNo" },
            { data: "refundId" },
            { data: "outRefundNo" },
            { data: "totalFee" },
            { data: "settlementTotalFee" },
            { data: "refundFee" },
            { data: "settlementRefundFee" },
            { data: "refundStatus" },
            { data: "successTime" },
            { data: "refundRecvAccout" },
            { data: "refundAccount" },
            { data: "refundRequestSource" },
        ]
    }));
});