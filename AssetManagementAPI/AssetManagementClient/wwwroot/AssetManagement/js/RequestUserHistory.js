function format(d) {
    // `d` is the original data object for the row
    return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
        '<tr>' +
        '<td>Request Id:</td>' +
        '<td>' + d.id + '</td>' +
        '</tr>' +
        '<tr>' +
        '<td>Employee' + "'s" + ' Id:</td>' +
        '<td>' + d.accountId + '</td>' +
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
    var data = $('#tabledatauserrequesthistory').DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            { extend: 'excel', text: '<i class="fas fa-file-excel" style="color:green;"></i>', titleAttr: 'Excel' },
            { extend: 'pdf', text: '<i class="fas fa-file-pdf" style="color:crimson;"></i>', titleAttr: 'PDF' },
            { extend: 'print', text: '<i class="fas fa-print"></i>', titleAttr: 'Print', exportOptions: {columns:[3,4,5,6,9]} }
        ],
        "ajax": {
            "url": "https://localhost:44395/API/RequestItems/UserRequest",
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
            { 'data': 'statusId' },
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
                "targets": 7
            },
            {
                "visible": false,
                "targets": 8
            },
            {
                "visible": false,
                "targets": 10
            }
        ],
        "order": [[10, 'desc']]
    });

    // Add event listener for opening and closing details
    $('#tabledatauserrequesthistory tbody').on('click', 'td.details-control', function () {
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
