// TABLE DATA ITEM
$(document).ready(function () {
    var dataItem = $('#tabledataitem').DataTable({
        "ajax": {
            "url": "https://localhost:44395/API/Items/DataItem",
            "type": "get",
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
            { 'data': 'name' },
            { 'data': 'id' },
            { 'data': 'quantity' },
            { 'data': 'category' },
            {
                'data': null,
                render: function (data, type, row, meta) {
                    return '<a href="javascript:void(0)" id="buttonUpdate" type="button" class="btn btn-warning" data-toggle="modal" title="Update Item"><i class="fas fa-edit"></i></a>'

                },
                'searchable': false,
                'orderable': false
            }
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
                "targets": 5,
                className: 'dt-body-center'
            }
        ],
        "order": [[1, 'asc']]

    });

    dataItem.on('order.dt search.dt', function () {
        dataItem.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});

// ADD ITEM
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
        Swal.fire('Success', 'Item Has Been Added', 'success');
        $('#addNewItem').hide;
        $("#name").val(null);
        $("#quantity").val(null);
        $("#categoryId").val(null);
        $('#tabledataitem').DataTable().ajax.reload();
            
    }).fail((error) => {
        Swal.fire('Error', 'Something Went Wrong', 'error');
    });
}

// UPDATE ITEM
$("#tabledataitem").on('click', '#buttonUpdate', function () {
    var data = $("#tabledataitem").DataTable().row($(this).parents('tr')).data();
    console.log(data);
    $('#idE').val(data.id);
    $('#nameE').val(data.name);
    $('#quantityE').val(data.quantity);
    $('#categoryIdE').val(data.categoryId);
    $("#editModalItem").modal("show");
    $("#editModalItem").on('click', '#btnCloseEditItem', function () {
        $("#editModalItem").modal("hide");
    })
    $("#editModalItem").on('click', '#editUser', function () {
        $("#editModalItem").modal("hide");
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
            Swal.fire('Success','Item Has Been Updated','success');
            $('#tabledataitem').DataTable().ajax.reload();

        }).fail((error) => {
            Swal.fire('Error', 'Something Went Wrong', 'error');
        });
    })

})

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