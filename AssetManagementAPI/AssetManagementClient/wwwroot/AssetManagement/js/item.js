$(document).ready(function () {
    var data = $('#tabledataitem').DataTable({
        //"dom": 'Bfrtip',
        //"buttons": [
        //    'copy', 'csv', 'excel', 'pdf', 'print'
        //],
        "ajax": {
            "url": "https://localhost:44395/API/Items",
            "type": "get",
            "datatype": "json",
            "dataSrc": "data"
        },
        "columns": [
            { 'data': null },
            { 'data': 'id' },
            { 'data': 'name' },
            { 'data': 'quantity' },
            { 'data': 'categoryId' },
            {
                'data': null,
                render: function (data, type, row, meta) {
                    return '<a href="javascript:void(0)" class="far fa-check-circle table-action text-dark" data-toggle="tooltip" data-placement="top" title="Approve" onclick="Get(\'' + row['id'] + '\')"></a> ' +
                        '<a href="javascript:void(0)" class="far fa-times-circle table-action text-danger" data-toggle="tooltip" data-placement="top" title="Reject" onclick="Reject(\'' + row['id'] + '\')"></a>'

                },
                'searchable': false,
                'orderable': false
            }
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
(function () {
    'use strict'
    var forms = document.querySelectorAll('.needs-validation')
    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }

                form.classList.add('was-validated')
            }, false)
        })
})()

function AddNewItem() {
    console.log("ok");
    var Item = new Object();
    Item.name = $('#name').val();
    Item.quantity = $('#quantity').val();
    Item.categoryId = $('#categoryId').val();
    $.ajax({
        url: 'https://localhost:44395/API/Items',
        type: "POST",
        data: Item
    }).done((result) => {
        if (result == 200) {
            swal('Success', 'Request Has Been Added, Cek Your Email', 'success');
            $('#addNewItem').modal('hide');
            $("#id").val(null);
            $("#quantity").val(null);
            $("#categoryId").val(null);
            table.ajax.reload();
        }
        else {
            swal('Error', 'Something Went Wrong', 'error');
        }
    }).fail((error) => {
        console.log(error)
    });
}
