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
            { 'data': 'itemId' },
            {
                'data': 'startDate',
                'render': function (data) {
                    return moment(data).format('DD MMM YYYY')
                }
            },
            {
                'data': 'endDate',
                'render': function (data) {
                    return moment(data).format('DD MMM YYYY')
                }
            },
            { 'data': 'quantity' },
            { 'data': 'notes' },
            { 'data': 'status' },
            {
                'data': "null",
                'render': function (data, type, row, meta) {
                    if (row.status == "Waiting") {
                        return "<button type='button' class='btn btn-primary' data-toggle='modal' data-target='#needsApproval' title='Detail Request' id='btnNeedsApproval'><i class='fas fa-check-square'></i></button>";
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
                "targets": [1],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [2],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [5],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [9],
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

// Ini Confirm Picked Up
$("#tableDataListReq").on('click', '#btnNeedsApproval', function () {
    var data = $("#tableDataListReq").DataTable().row($(this).parents('tr')).data();
    console.log(data);
    $('#userId_emp').val(data.accountId);
    $('#name_emp').val(data.name);
    $('#req_id').val(data.id);
    $('#item_name').val(data.item);
    $('#req_date').val(moment(data.startDate).format('DD MMM YYYY') + " to " + moment(data.endDate).format('DD MMM YYYY'));
    $('#req_quantity').val(data.quantity);
    $('#req_notes').val(data.notes);
    $('#startDateE').val(data.startDate.slice(0, 10));
    $('#endDateE').val(data.endDate.slice(0, 10));
    $("#needsApproval").modal("show");
    $("#needsApproval").on('click', '#btnNeedsApprovalClose', function () {
        $("#needsApproval").modal("hide");
    })
    $("#needsApproval").on('click', '#btnApproveReq', function () {
        $("#needsApproval").modal("hide");
        Swal.fire({
            title: 'You will approve the request?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#27e65a',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Approve it!'
        }).then((result) => {
            if (result.isConfirmed) {
                var obj = new Object();
                obj.Id = data.id;
                obj.AccountId = data.accountId;
                obj.ItemId = data.itemId;
                obj.StartDate = data.startDate.slice(0, 10);
                obj.EndDate = data.endDate.slice(0, 10);
                obj.Quantity = data.quantity;
                obj.Notes = data.notes;
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
            }
        });
    })
    $("#needsApproval").on('click', '#btnRejectReq', function () {
        $("#needsApproval").modal("hide");
        Swal.fire({
            title: 'You will reject the request?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Reject it!'
        }).then((result) => {
            if (result.isConfirmed) {
                var obj = new Object();
                obj.Id = data.id;
                obj.AccountId = data.accountId;
                obj.ItemId = data.itemId;
                obj.StartDate = data.startDate.slice(0, 10);
                obj.EndDate = data.endDate.slice(0, 10);
                obj.Quantity = data.quantity;
                obj.Notes = data.notes;
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
                        'Data failed to reject !',
                        'error'
                    );
                });
            }
        });
    })

})