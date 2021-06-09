$(document).ready(function () {
    var data = $('#tabledatauserrequesttaken').DataTable({
        "ajax": {
            "url": "https://localhost:44395/API/RequestItems/UserRequestTake",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            { 'data': null },
            { 'data': 'id' },
            { 'data': 'item' },
            { 'data': 'name' },
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
        "columnDefs": [
            {
                "searchable": false,
                "orderable": false,
                "targets": 0
            },
            {
                "visible": false,
                "targets": 1
            }
        ],
        "order": [[1, 'desc']]

    });
    data.on('order.dt search.dt', function () {
        data.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});

// Ini Confirm Picked Up
$("#tabledatauserrequesttaken").on('click', '#buttonTakeAnAsset', function () {
    var data = $("#tabledatauserrequesttaken").DataTable().row($(this).parents('tr')).data();
    console.log(data);
    $('#idE').val(data.id);
    $('#accountIdE').val(data.accountId);
    $('#itemIdE').val(data.itemId);
    $('#startDateE').val(data.startDate.slice(0, 10));
    $('#endDateE').val(data.endDate.slice(0, 10));
    $('#quantityE').val(data.quantity);
    $('#notesE').val(data.notes);

    $("#takeAnAsset").modal("show");
    $("#takeAnAsset").on('click', '#taken', function () {

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
            Swal.fire('Success','Item Already Picked Up','success'
            );
            $('#tabledatauserrequesttaken').DataTable().ajax.reload();

        }).fail((error) => {
            Swal.fire('Error', 'Something Went Wrong', 'error');
        });
    })

})
