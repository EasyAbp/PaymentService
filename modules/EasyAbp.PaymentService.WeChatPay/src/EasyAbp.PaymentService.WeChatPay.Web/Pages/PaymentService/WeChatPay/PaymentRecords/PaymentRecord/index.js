$(function () {

    var l = abp.localization.getResource('EasyAbpPaymentServiceWeChatPay');

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
            { data: "paymentId" },
            { data: "appId" },
            { data: "mchId" },
            { data: "outTradeNo" },
            { data: "transactionId" },
            { data: "tradeType" },
            { data: "bankType" },
            { data: "attach" },
            { data: "tradeState" },
            { data: "tradeStateDesc" },
            { data: "successTime", dataFormat: 'datetime' },
            { data: "payer" },
            { data: "amount" },
            { data: "sceneInfo" },
            { data: "promotionDetail" },
            { data: "returnCode" },
            { data: "returnMsg" },
            { data: "deviceInfo" },
            { data: "resultCode" },
            { data: "errCode" },
            { data: "errCodeDes" },
            { data: "openid" },
            { data: "isSubscribe" },
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
            { data: "timeEnd" },
        ]
    }));
});