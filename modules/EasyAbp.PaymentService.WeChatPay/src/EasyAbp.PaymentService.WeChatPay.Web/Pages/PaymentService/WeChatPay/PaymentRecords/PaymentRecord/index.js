$(function () {

    var l = abp.localization.getResource('WeChatPay');

    var service = easyAbp.paymentService.weChatPay.paymentRecords.paymentRecord;

    var dataTable = $('#PaymentRecordTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
            { data: "deviceInfo" },
            { data: "nonceStr" },
            { data: "sign" },
            { data: "signType" },
            { data: "resultCode" },
            { data: "errCode" },
            { data: "errCodeDes" },
            { data: "openid" },
            { data: "isSubscribe" },
            { data: "tradeType" },
            { data: "bankType" },
            { data: "totalFee" },
            { data: "settlementTotalFee" },
            { data: "feeType" },
            { data: "cashFee" },
            { data: "cashFeeType" },
            { data: "couponFee" },
            { data: "couponCount" },
            { data: "couponTypes" },
            { data: "couponIds" },
            { data: "couponFees" },
            { data: "transactionId" },
            { data: "outTradeNo" },
            { data: "attach" },
            { data: "timeEnd" },
        ]
    }));
});