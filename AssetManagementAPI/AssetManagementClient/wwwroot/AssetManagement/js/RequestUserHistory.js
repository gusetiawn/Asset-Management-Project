﻿$(document).ready(function () {
    var data = $('#tabledatauserrequesthistory').DataTable({
        "ajax": {
            "url": "https://localhost:44395/API/RequestItems/UserRequest",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            { 'data': null },
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
            { 'data': 'status' }
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