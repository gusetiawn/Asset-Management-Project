$(document).ready(function () {
    var data = $('#tabledatauserrequestreturn').DataTable({
        "ajax": {
            "url": "https://localhost:44395/API/RequestItems/UserRequestReturn",
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
                    return ' <button class="btn btn-outline-primary" data-toggle="modal" data-target="#returnAsset" id="btngetid"><i class="fas fa-check-square"></i></button> '

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

$("#tabledatauserrequestreturn").on('click', '#btngetid', function () {
    var data = $("#tabledatauserrequestreturn").DataTable().row($(this).parents('tr')).data();
    $('#requestItemId').val(data.id);
    console.log(data);
})

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
        Swal.fire('Success','Item Has Been Returned','success');
        $('#tabledatauserrequestreturn').DataTable().ajax.reload();

    }).fail((error) => {
        Swal.fire('Error', 'Something Went Wrong', 'error');
    });
}