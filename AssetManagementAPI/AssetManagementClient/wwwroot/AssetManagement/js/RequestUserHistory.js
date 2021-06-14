$(document).ready(function () {
    var data = $('#tabledatauserrequesthistory').DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            { extend: 'excel', text: '<i class="fas fa-file-excel" style="color:green;"></i>', titleAttr: 'Excel' },
            { extend: 'pdf', text: '<i class="fas fa-file-pdf" style="color:crimson;"></i>', titleAttr: 'PDF' },
            { extend: 'print', text: '<i class="fas fa-print"></i>', titleAttr: 'Print' }
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
            },
            {
                "visible": false,
                "targets": 6
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
