$(function () {

    var l = abp.localization.getResource('EasyAbpPaymentService');

    var service = easyAbp.paymentService.refunds.refund;

    var dataTable = $('#RefundTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            },
                        ]
                }
            },
            { data: "paymentId" },
            { data: "paymentItemId" },
            { data: "refundPaymentMethod" },
            { data: "externalTradingCode" },
            { data: "currency" },
            { data: "refundAmount" },
            { data: "customerRemark" },
            { data: "staffRemark" },
            { data: "completedTime" },
            { data: "cancelledTime" },
        ]
    }));
});