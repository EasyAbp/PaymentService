$(function () {

    var l = abp.localization.getResource('EasyAbpPaymentServicePrepayment');

    var service = easyAbp.paymentService.prepayment.transactions.transaction;

    var dataTable = $('#TransactionTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[0, "asc"]],
        ajax: abp.libs.datatables.createAjax(service.getList, function () {
            return { accountId: accountId };
        }),
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
            { data: "transactionType" },
            { data: "actionName" },
            { data: "paymentMethod" },
            { data: "externalTradingCode" },
            { data: "currency" },
            { data: "changedBalance" },
            { data: "originalBalance" },
        ]
    }));
});