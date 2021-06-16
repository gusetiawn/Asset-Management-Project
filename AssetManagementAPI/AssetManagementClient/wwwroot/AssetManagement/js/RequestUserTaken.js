function format(dataTaken) {
    // `d` is the original data object for the row
    return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
        '<tr>' +
        '<td>Request Id:</td>' +
        '<td>' + dataTaken.id + '</td>' +
        '</tr>' +
        '<tr>' +
        '<td>Employee' + "'s" + ' Id:</td>' +
        '<td>' + dataTaken.accountId + '</td>' +
        '</tr>' +
        '<tr>' +
        '<td>Quantity of Requested Item:</td>' +
        '<td>' + dataTaken.quantity + ' items</td>' +
        '</tr>' +
        '<tr>' +
        '<td>Notes for the Request:</td>' +
        '<td>' + dataTaken.notes + '</td>' +
        '</tr>' +
        '</table>';
}

$(document).ready(function () {
    var data = $('#tabledatauserrequesttaken').DataTable({
        "ajax": {
            "url": "https://localhost:44395/API/RequestItems/UserRequestTake",
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
            { 'data': 'quantity' },
            { 'data': 'notes' },
            { 'data': 'status' },
            {
                'data': null,
                render: function (data, type, row, meta) {
                    return ' <button class="btn btn-primary" type="button" id="buttonTakeAnAsset" data-toggle="tooltip" data-placement="top" title="Verification"><i class="fas fa-check-square"></i></button> '

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
                "targets": 2
            },
            {
                "visible": false,
                "targets": 5
            },
            {
                "visible": false,
                "targets": 8
            },
            {
                "visible": false,
                "targets": 9
            },
            {
                "targets": 11,
                className: 'dt-body-center'
            }
        ],
        "order": [[1, 'desc']]

    });

    // Add event listener for opening and closing details
    $('#tabledatauserrequesttaken tbody').on('click', 'td.details-control', function () {
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

// Ini Confirm Picked Up
$("#tabledatauserrequesttaken").on('click', '#buttonTakeAnAsset', function () {
    var data = $("#tabledatauserrequesttaken").DataTable().row($(this).parents('tr')).data();
    console.log(data);
    $('#userId_taken').val(data.accountId);
    $('#name_taken').val(data.name);
    $('#reqId_taken').val(data.id);
    $('#itemName_taken').val(data.item);
    $('#reqDate_taken').val(moment(data.startDate).format('DD-MM-YYYY') + " to " + moment(data.endDate).format('DD-MM-YYYY'));
    $('#reqQty_taken').val(data.quantity);
    $('#reqNotes_taken').val(data.notes);
    $('#itemIdE').val(data.itemId);
    $('#startDateE').val(moment(data.startDate).format('DD-MM-YYYY'));
    $('#endDateE').val(moment(data.endDate).format('DD-MM-YYYY'));
    $("#takeAnAsset").modal("show");
    $("#takeAnAsset").on('click', '#btnTakenClose', function () {
        $("#takeAnAsset").modal("hide");
    })
    $("#takeAnAsset").on('click', '#taken', function () {
        $("#takeAnAsset").modal("hide");
        var edit = new Object();
        console.log(edit);
        //edit.id = $('#idE').val();
        //edit.accountId = $('#accountIdE').val();
        //edit.itemId = $('#itemIdE').val();
        //edit.startDate = $('#startDateE').slice(0, 10).val();
        //edit.endDate = $('#endDateE').slice(0, 10).val();
        //edit.quantity = $('#quantityE').val();
        //edit.notes = $('#notesE').val();

        edit.id = data.id;
        edit.accountId = data.accountId;
        edit.itemId = data.itemId;
        edit.startDate = data.startDate.slice(0, 10);
        edit.endDate = data.endDate.slice(0, 10);
        edit.quantity = data.quantity;
        edit.notes = data.notes;

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
