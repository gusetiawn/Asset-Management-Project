$(document).ready(function () {
    var statusWaiting = $('#tableDataListReq').DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
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
                    return data.slice(0, 10);
                }
            },
            {
                'data': 'endDate',
                'render': function (data) {
                    return data.slice(0, 10);
                }
            },
            { 'data': 'quantity' },
            { 'data': 'notes' },
            { 'data': 'status' },
            {
                'data': "null",
                'render': function (data, type, row, meta) {
                    if (row.status == "Waiting for Approval") {
                        return "<button type='button' class='btn' data-toggle='modal' data-target='#approveModal' onclick='findPerson(" + '"' + row.id + '"' + ")' data-toggle='tooltip' data-placement='top' title='Approve'><span style='color: lime;'><i class='far fa-check-circle'></i></span></button><button type='button' class='btn' data-toggle='modal' data-target='#rejectModal' onclick='findPerson(" + '"' + row.id + '"' + ")' data-toggle='tooltip' data-placement='top' title='Reject'><span style='color: Tomato;'><i class='far fa-times-circle'></i></span></button>";
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

function findPerson(ReqId) {
    var btnApprove = "";
    btnApprove = '<button type="button" class="btn btn-primary" data-dismiss="modal" onclick="approvedRequest(' + "'" + ReqId + "'" + ')">Yes</button><button type="button" class="btn btn-danger" data-dismiss="modal">No</button>'
    $(".approveBtn").html(btnApprove);

    var btnReject = "";
    btnReject = '<button type="button" class="btn btn-primary" data-dismiss="modal" onclick="rejectRequest(' + "'" + ReqId + "'" + ')">Yes</button><button type="button" class="btn btn-danger" data-dismiss="modal">No</button>'
    $(".rejectBtn").html(btnReject);
}

function approvedRequest(IdReq) {
    $.ajax({
        url: "https://localhost:44395/API/RequestItems/Id=" + IdReq
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
                'Good job!',
                'Data successfully approved !',
                'success'
            );
            $("#tableDataListReq").DataTable().ajax.reload();

        }).fail((notsuccess) => {
            Swal.fire(
                'Error!',
                'Data failed updated !',
                'error'
            );
        });
    }).fail((error) => {
        Swal.fire(
            'Error!',
            'Data failed updated !',
            'error'
        );
    });
}

function rejectRequest(IdReq) {
    $.ajax({
        url: "https://localhost:44395/API/RequestItems/Id=" + IdReq
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
                'Good job!',
                'Data successfully rejected !',
                'success'
            );
            $("#tableDataListReq").DataTable().ajax.reload();

        }).fail((notsuccess) => {
            Swal.fire(
                'Error!',
                'Data failed updated !',
                'error'
            );
        });
    }).fail((error) => {
        Swal.fire(
            'Error!',
            'Data failed updated !',
            'error'
        );
    });
}