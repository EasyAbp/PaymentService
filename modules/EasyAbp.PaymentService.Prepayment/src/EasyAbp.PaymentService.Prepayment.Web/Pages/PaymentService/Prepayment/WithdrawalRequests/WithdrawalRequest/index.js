$(function () {

    var l = abp.localization.getResource('EasyAbpPaymentServicePrepayment');

    var service = easyAbp.paymentService.prepayment.withdrawalRequests.withdrawalRequest;

    var dataTable = $('#WithdrawalRequestTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                                text: l('Approve'),
                                visible: abp.auth.isGranted('EasyAbp.PaymentService.Prepayment.WithdrawalRequest.Review'),
                                confirmMessage: function (data) {
                                    return l('WithdrawalRequestApproveConfirmationMessage', data.record.id);
                                },
                                action: function (data) {
                                    if (data.record.reviewTime) {
                                        abp.notify.error(l('WithdrawalRequestHasBeenReviewed'));
                                        return;
                                    }
                                    service.review(data.record.id, { isApproved: true }).then(function () {
                                        abp.notify.info(l('SuccessfullyApproved'));
                                        dataTable.ajax.reload();
                                    });
                                }
                            },
                            {
                                text: l('Reject'),
                                visible: abp.auth.isGranted('EasyAbp.PaymentService.Prepayment.WithdrawalRequest.Review'),
                                confirmMessage: function (data) {
                                    return l('WithdrawalRequestRejectConfirmationMessage', data.record.id);
                                },
                                action: function (data) {
                                    if (data.record.reviewTime) {
                                        abp.notify.error(l('WithdrawalRequestHasBeenReviewed'));
                                        return;
                                    }
                                    service.review(data.record.id, { isApproved: true }).then(function () {
                                        abp.notify.info(l('SuccessfullyRejected'));
                                        dataTable.ajax.reload();
                                    });
                                }
                            }
                        ]
                }
            },
            { data: "accountId" },
            { data: "accountUserId" },
            { data: "amount" },
            { data: "reviewTime", dataFormat: 'datetime' },
            { data: "reviewerUserId" },
            { data: "isApproved" },
        ]
    }));
});