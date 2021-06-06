$(document).ready(function () {
    var data = $('#tabledatauserrequest').DataTable({
        "ajax": {
            "url": "https://localhost:44395/API/RequestItems/UserRequestTake",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            { 'data': null },
            { 'data': 'id' },
            { 'data': 'item' },
            { 'data': 'accountId' },
            {
                'data': 'startDate',
                render: function (data, type, row) {
                    return data.slice(0, 10)
                }
            },
            {
                'data': 'endDate',
                render: function (data, type, row) {
                    return data.slice(0, 10)
                }
            },
            { 'data': 'notes' },
            { 'data': 'quantity' },
            { 'data': 'status' },
            {
                'data': null,
                render: function (data, type, row, meta) {
                    return ' <button class="btn btn-primary" type="button" id="buttonTakeAnAsset"><i class="fas fa-check-square"></i></button> '

                },
                'searchable': false,
                'orderable': false
            }
        ],
        "columnDefs": [{
            "searchable": false,
            "orderable": false,
            "targets": 0
        }],
        "order": [[1, 'asc']]

    });
    data.on('order.dt search.dt', function () {
        data.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});

function AddNewRequest() {
    var req = new Object();
    req.accountId = $('#accountId').val();
    req.itemId = $('#itemId').val();
    req.startDate = $('#startDate').val();
    req.endDate = $('#endDate').val();
    req.quantity = $('#quantity').val();
    req.notes = $('#notes').val();
    console.log(req);
    $.ajax({
        type: "POST",
        url: 'https://localhost:44395/API/RequestItems/NewRequest',
        data: JSON.stringify(req),
        contentType: "application/json; charset=utf-8",
        datatype: "json"
    }).done((result) => {
        Swal.fire(
            'Success',
            'Item Has Been Added, Cek Your Email',
            'success'
        );
        $('#tabledatauserrequest').DataTable().ajax.reload();

    }).fail((error) => {
        Swal.fire('Error', 'Something Went Wrong', 'error');
    });
}

$("#tabledatauserrequest").on('click', '#buttonTakeAnAsset', function () {
    var data = $("#tabledatauserrequest").DataTable().row($(this).parents('tr')).data();
    console.log(data);
    $('#idE').val(data.id);
    $('#accountIdE').val(data.accountId);
    $('#itemIdE').val(data.itemId);
    $('#startDateE').val(data.startDate.slice(0,10));
    $('#endDateE').val(data.endDate.slice(0, 10));
    $('#quantityE').val(data.quantity);
    $('#notesE').val(data.notes);

    $("#takeAnAsset").modal("show");
    $("#takeAnAsset").on('click', '#taked', function () {

        var edit = new Object();
        console.log(edit);
        edit.id = $('#idE').val();
        edit.accountId = $('#accountIdE').val();
        edit.itemId = $('#itemIdE').val();
        edit.startDate = $('#startDateE').slice(0, 10).val();
        edit.endDate = $('#endDateE').slice(0, 10).val();
        edit.quantity = $('#quantityE').val();
        edit.notes = $('#notesE').val();

        $.ajax({
            url: 'https://localhost:44395/API/RequestItems/TakeAnAsset',
            type: "PUT",
            data: JSON.stringify(edit),
            contentType: "application/json; charset=utf-8",
            datatype: "json"
        }).done((result) => {
            Swal.fire(
                'Success',
                'Item Has Been Added, Cek Your Email',
                'success'
            );
            $('#tabledataitem').DataTable().ajax.reload();

        }).fail((error) => {
            Swal.fire('Error', 'Something Went Wrong', 'error');
        });
    })

})

$(document).ready(function () {
    var data = $('#tabledatauserrequesttaked').DataTable({
        "ajax": {
            "url": "https://localhost:44395/API/RequestItems/UserRequestReturn",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            { 'data': null },
            { 'data': 'id' },
            { 'data': 'item' },
            { 'data': 'accountId' },
            {
                'data': 'startDate',
                render: function (data, type, row) {
                    return data.slice(0, 10)
                }
            },
            {
                'data': 'endDate',
                render: function (data, type, row) {
                    return data.slice(0, 10)
                }
            },
            { 'data': 'notes' },
            { 'data': 'quantity' },
            { 'data': 'status' },
            {
                'data': null,
                render: function (data, type, row, meta) {
                    return ' <button class="btn btn-outline-primary" data-toggle="modal" data-target="#returnAsset"><i class="fas fa-check-square"></i></button> '

                },
                'searchable': false,
                'orderable': false
            }
        ],
        "columnDefs": [{
            "searchable": false,
            "orderable": false,
            "targets": 0
        }],
        "order": [[1, 'asc']]

    });
    data.on('order.dt search.dt', function () {
        data.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});

function ReturnAnAsset() {
    var returnAsset = new Object();
    returnAsset.requestItemId = $('#requestItemId').val();
    returnAsset.penalty = $('#penalty').val();
    returnAsset.notes = $('#note').val();
    console.log(returnAsset);
    $.ajax({
        type: "POST",
        url: 'https://localhost:44395/API/ReturnItems/NewRequest',
        data: JSON.stringify(returnAsset),
        contentType: "application/json; charset=utf-8",
        datatype: "json"
    }).done((result) => {
        Swal.fire(
            'Success',
            'Item Has Been Added, Cek Your Email',
            'success'
        );
        $('#tabledatauserrequesttaked').DataTable().ajax.reload();

    }).fail((error) => {
        Swal.fire('Error', 'Something Went Wrong', 'error');
    });
}

$(document).ready(function () {
    var data = $('#tabledatauserrequesthistory').DataTable({
        "ajax": {
            "url": "https://localhost:44395/API/RequestItems/UserRequest",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            { 'data': null },
            { 'data': 'id' },
            { 'data': 'item' },
            { 'data': 'accountId' },
            {
                'data': 'startDate',
                render: function (data, type, row) {
                    return data.slice(0, 10)
                }
            },
            {
                'data': 'endDate',
                render: function (data, type, row) {
                    return data.slice(0, 10)
                }
            },
            { 'data': 'notes' },
            { 'data': 'quantity' },
            { 'data': 'status' },
        ],
        "columnDefs": [{
            "searchable": false,
            "orderable": false,
            "targets": 0
        }],
        "order": [[1, 'asc']]

    });
    data.on('order.dt search.dt', function () {
        data.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});
