$(document).ready(function () {
    var statusWaiting = $('#tableDataListReq').DataTable({
        "ajax": {
            "url": "https://localhost:44395/API/RequestItems/RequestNeedsApproval",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            {
                'data': null,
                'render': function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { 'data': 'id' },
            { 'data': 'accountId' },
            { 'data': 'name' },
            { 'data': 'item' },
            {
                'data': 'startDate',
                'render': function (data) {
                    return moment(data).format('DD-MM-YYYY')
                }
            },
            {
                'data': 'endDate',
                'render': function (data) {
                    return moment(data).format('DD-MM-YYYY')
                }
            },
            { 'data': 'quantity' },
            { 'data': 'notes' },
            { 'data': 'status' },
            {
                'data': "null",
                'render': function (data, type, row, meta) {
                    if (row.status == "Waiting for Approval") {
                        return "<button type='button' class='btn' onclick='approveRequest(" + '"' + row.id + '"' + ")' data-toggle='tooltip' data-placement='top' title='Approve'><span style='color: lime;'><i class='far fa-check-circle'></i></span></button><button type='button' class='btn' data-toggle='modal' data-target='#rejectModal' onclick='rejectRequest(" + '"' + row.id + '"' + ")' data-toggle='tooltip' data-placement='top' title='Reject'><span style='color: Tomato;'><i class='far fa-times-circle'></i></span></button>";
                    }
                    else {
                        return null;
                    }
                }
            }
            
        ],
        "columnDefs": [
            {
                "searchable": false,
                "orderable": false,
                "targets": 0
            },
            {
                "targets": [2],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [8],
                "visible": false,
                "searchable": false
            }
        ],
        "order": [[1, "desc"]]
    });
    statusWaiting.on('order.dt search.dt', function () {
        statusWaiting.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});

function approveRequest(ReqId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#27e65a',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Approve it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "https://localhost:44395/API/RequestItems/Id=" + ReqId
            }).done((result) => {
                var obj = new Object();
                obj.Id = result[0].id;
                obj.AccountId = result[0].accountId;
                obj.ItemId = result[0].itemId;
                obj.StartDate = result[0].startDate;
                obj.EndDate = result[0].endDate;
                obj.Quantity = result[0].quantity;
                obj.Notes = result[0].notes;
                $.ajax({
                    type: "PUT",
                    url: "https://localhost:44395/API/RequestItems/Approve",
                    data: JSON.stringify(obj),
                    contentType: "application/json; charset=utf-8",
                    datatype: "json"
                }).done((success) => {
                    Swal.fire(
                        'Approved!',
                        'The Request has been Approved.',
                        'success'
                    );
                    $("#tableDataListReq").DataTable().ajax.reload();

                }).fail((notsuccess) => {
                    Swal.fire(
                        'Error!',
                        'Data failed to approve !',
                        'error'
                    );
                });
            }).fail((error) => {
                Swal.fire(
                    'Error!',
                    'Data failed to approve !',
                    'error'
                );
            });
        }
    });
}

function rejectRequest(ReqId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Reject it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "https://localhost:44395/API/RequestItems/Id=" + ReqId
            }).done((result) => {
                var obj = new Object();
                obj.Id = result[0].id;
                obj.AccountId = result[0].accountId;
                obj.ItemId = result[0].itemId;
                obj.StartDate = result[0].startDate;
                obj.EndDate = result[0].endDate;
                obj.Quantity = result[0].quantity;
                obj.Notes = result[0].notes;
                $.ajax({
                    type: "PUT",
                    url: "https://localhost:44395/API/RequestItems/Reject",
                    data: JSON.stringify(obj),
                    contentType: "application/json; charset=utf-8",
                    datatype: "json"
                }).done((success) => {
                    Swal.fire(
                        'Rejected!',
                        'The Request has been Rejected.',
                        'success'
                    );
                    $("#tableDataListReq").DataTable().ajax.reload();

                }).fail((notsuccess) => {
                    Swal.fire(
                        'Error!',
                        'Request failed to reject !',
                        'error'
                    );
                });
            }).fail((error) => {
                Swal.fire(
                    'Error!',
                    'Data failed to reject !',
                    'error'
                );
            });
        }
    });
}