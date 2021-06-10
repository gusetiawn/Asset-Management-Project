﻿$(document).ready(function () {
    var statusWaiting = $('#tableDataReqHistory').DataTable({
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
            { 'data': 'status' }
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