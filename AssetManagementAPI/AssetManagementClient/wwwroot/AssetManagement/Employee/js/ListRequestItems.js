$(document).ready(function () {
    var dataReqItem = $('#tableDataReqItem').DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "ajax": {
            "url": "https://localhost:44395/API/RequestItems/E001",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            {
                'data': 'null',
                'render': function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
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
        "columnDefs": [{
            "searchable": false,
            "orderable": false,
            "targets": 0
        }],
        

    });
    dataReqItem.on('order.dt search.dt', function () {
        dataReqItem.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});

//(function () {
//    'use strict'
//    var forms = document.querySelectorAll('.needs-validation')
//    Array.prototype.slice.call(forms)
//        .forEach(function (form) {
//            form.addEventListener('submit', function (event) {
//                if (!form.checkValidity()) {
//                    event.preventDefault()
//                    event.stopPropagation()
//                }

//                form.classList.add('was-validated')
//            }, false)
//        })
//})()

//function AddNewItem() {
//    console.log("ok");
//    var Item = new Object();
//    Item.name = $('#name').val();
//    Item.quantity = $('#quantity').val();
//    Item.categoryId = $('#categoryId').val();
//    $.ajax({
//        type: "POST",
//        url: 'https://localhost:44395/API/Items',
//        data: JSON.stringify(Item),
//        contentType: "application/json; charset=utf-8",
//        datatype: "json"
//    }).done((result) => {
//            Swal.fire(
//                'Success',
//                'Item Has Been Added, Cek Your Email',
//                'success'
//            );
//            $('#addNewItem').modal('hide');
//            $("#id").val(null);
//            $("#quantity").val(null);
//            $("#categoryId").val(null);
//            $('#tabledata').DataTable().ajax.reload();
            
//    }).fail((error) => {
//        Swal.fire('Error', 'Something Went Wrong', 'error');
//    });
//}
