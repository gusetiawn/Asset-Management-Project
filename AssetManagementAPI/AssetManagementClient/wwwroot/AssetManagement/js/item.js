$(document).ready(function () {
    var data = $('#tabledataitem').DataTable({
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
                    return '<a href="javascript:void(0)" id="buttonUpdate" type="button" class="btn btn-warning" data-toggle="modal"><i class="fas fa-edit"></i></a>'

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
    var Item = new Object();
    Item.name = $('#name').val();
    Item.quantity = $('#quantity').val();
    Item.categoryId = $('#categoryId').val();
    $.ajax({
        type: "POST",
        url: 'https://localhost:44395/API/Items',
        data: JSON.stringify(Item),
        contentType: "application/json; charset=utf-8",
        datatype: "json"
    }).done((result) => {
            Swal.fire(
                'Success',
                'Item Has Been Added, Cek Your Email',
                'success'
            );
            $('#addNewItem').modal('hide');
            $("#name").val(null);
            $("#quantity").val(null);
            $("#categoryId").val(null);
            $('#tabledata').DataTable().ajax.reload();
            
    }).fail((error) => {
        Swal.fire('Error', 'Something Went Wrong', 'error');
    });
}

//UPDATE
$("#tabledataitem").on('click', '#buttonUpdate', function () {
    var data = $("#tabledataitem").DataTable().row($(this).parents('tr')).data();
    console.log(data);
    $('#idE').val(data.id);
    $('#nameE').val(data.name);
    $('#quantityE').val(data.quantity);
    $('#categoryIdE').val(data.categoryId);

    $("#editModalItem").modal("show");
    $("#editModalItem").on('click', '#editUser', function () {

        var edit = new Object();
        console.log(edit);
        edit.id = $('#idE').val();
        edit.name = $('#nameE').val();
        edit.quantity = $('#quantityE').val();
        edit.categoryId = $('#categoryIdE').val();
        
        $.ajax({
            url: 'https://localhost:44395/API/Items',
            type: "PUT",
            data: JSON.stringify(edit),
            contentType: "application/json; charset=utf-8",
            datatype: "json"
        }).done((result) => {
            Swal.fire(
                'Success',
                'Item Has Been Added, Cek Your Email',
                'success'
            );
            $('#tabledataitem').DataTable().ajax.reload();

        }).fail((error) => {
            Swal.fire('Error', 'Something Went Wrong', 'error');
        });
    })

})