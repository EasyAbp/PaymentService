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
        order: [[0, "asc"]],
        ajax: abp.libs.datatables.createAjax(service.getList, function () {
            return { userId: userId }
        }),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l('Transaction'),
                                action: function (data) {
                                    document.location.href = document.location.origin + '/PaymentService/Prepayment/Transactions/Transaction?AccountId=' + data.record.id;
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

    $('#search-button').click(function (e) {
        e.preventDefault();
        document.location.href = document.location.origin + document.location.pathname + '?userId=' + $('#UserId').val();
    })
});