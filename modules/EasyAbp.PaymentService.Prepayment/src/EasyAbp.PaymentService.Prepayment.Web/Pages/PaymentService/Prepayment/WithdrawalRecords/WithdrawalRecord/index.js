$(function () {

    var l = abp.localization.getResource('EasyAbpPaymentServicePrepayment');

    var service = easyAbp.paymentService.prepayment.withdrawalRecords.withdrawalRecord;

    var dataTable = $('#WithdrawalRecordTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[0, "asc"]],
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
            { data: "accountId" },
            { data: "withdrawalMethod" },
            { data: "amount" },
            { data: "completionTime", dataFormat: 'datetime' },
            { data: "cancellationTime", dataFormat: 'datetime' },
            { data: "resultErrorCode" },
            { data: "resultErrorMessage" },
        ]
    }));
});