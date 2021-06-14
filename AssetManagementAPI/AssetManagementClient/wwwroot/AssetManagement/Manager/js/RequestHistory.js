$(document).ready(function () {
    var statusWaiting = $('#tableDataReqHistory').DataTable({
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
                    return moment(data).format('DD-MM-YYYY')
                }
            },
            {
                'data': 'endDate',
                'render': function (data) {
                    return moment(data).format('DD-MM-YYYY')
                }
            },
            { 'data': 'quantity' },
            { 'data': 'notes' },
            { 'data': 'status' },
            { 'data': 'statusId' }
        ],
        "columnDefs": [
            {
                "searchable": false,
                "orderable": false,
                "targets": 0
            },
            {
                "targets": [2],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [8],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [10],
                "visible": false,
                "searchable": false
            }
        ],
        "order": [[10, "desc"]]
    });
    statusWaiting.on('order.dt search.dt', function () {
        statusWaiting.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});