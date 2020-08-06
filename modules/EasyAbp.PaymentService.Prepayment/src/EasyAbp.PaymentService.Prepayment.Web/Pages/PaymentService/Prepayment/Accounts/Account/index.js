$(function () {

    var l = abp.localization.getResource('EasyAbpPaymentServicePrepayment');

    var service = easyAbp.paymentService.prepayment.accounts.account;
    var changeBalanceModal = new abp.ModalManager(abp.appPath + 'PaymentService/Prepayment/Accounts/Account/ChangeBalanceModal');
    var changeLockedBalanceModal = new abp.ModalManager(abp.appPath + 'PaymentService/Prepayment/Accounts/Account/ChangeLockedBalanceModal');

    var dataTable = $('#AccountTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            {
                                text: l('ChangeAccountBalance'),
                                action: function (data) {
                                    changeBalanceModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('ChangeAccountLockedBalance'),
                                action: function (data) {
                                    changeLockedBalanceModal.open({ id: data.record.id });
                                }
                            }
                        ]
                }
            },
            { data: "accountGroupName" },
            { data: "userId" },
            { data: "balance" },
            { data: "lockedBalance" },
        ]
    }));

    changeBalanceModal.onResult(function () {
        dataTable.ajax.reload();
    });

    changeLockedBalanceModal.onResult(function () {
        dataTable.ajax.reload();
    });
});