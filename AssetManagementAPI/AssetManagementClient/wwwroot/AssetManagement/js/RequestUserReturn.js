function format(d) {
    // `d` is the original data object for the row
    return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
        '<tr>' +
        '<td>Request Id:</td>' +
        '<td>' + d.id + '</td>' +
        '</tr>' +
        '<tr>' +
        '<td>Quantity of Requested Item:</td>' +
        '<td>' + d.quantity + ' items</td>' +
        '</tr>' +
        '<tr>' +
        '<td>Notes for the Request:</td>' +
        '<td>' + d.notes + '</td>' +
        '</tr>' +
        '</table>';
}

$(document).ready(function () {
    var data = $('#tabledatauserrequestreturn').DataTable({
        "ajax": {
            "url": "https://localhost:44395/API/RequestItems/UserRequestReturn",
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
            { 'data': 'item' },
            { 'data': 'name' },
            {
                'data': 'startDate',
                render: function (data, type, row) {
                    return moment(data).format('DD-MM-YYYY')
                }
            },
            {
                'data': 'endDate',
                render: function (data, type, row) {
                    return moment(data).format('DD-MM-YYYY')
                }
            },
            { 'data': 'notes' },
            { 'data': 'quantity' },
            { 'data': 'status' },
            {
                'data': null,
                render: function (data, type, row, meta) {
                    return ' <button class="btn btn-primary" data-toggle="modal" data-target="#returnAsset" id="btngetid" data-toggle="tooltip" data-placement="top" title="Verification"><i class="fas fa-check-square"></i></button> '

                },
                'searchable': false,
                'orderable': false
            },
            {
                'className': 'details-control',
                'orderable': false,
                'data': null,
                'defaultContent': ''
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
            },
            {
                "visible": false,
                "targets": 6
            },
            {
                "visible": false,
                "targets": 7
            },
            {
                "targets": 9,
                className: 'dt-body-center'
            }
        ],
        "order": [[1, 'desc']]
    });

    // Add event listener for opening and closing details
    $('#tabledatauserrequestreturn tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = data.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
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