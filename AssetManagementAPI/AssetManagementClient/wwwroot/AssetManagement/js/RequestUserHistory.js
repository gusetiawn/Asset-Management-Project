$(document).ready(function () {
    var data = $('#tabledatauserrequesthistory').DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "ajax": {
            "url": "https://localhost:44395/API/RequestItems/UserRequest",
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
            { 'data': 'status' }
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
