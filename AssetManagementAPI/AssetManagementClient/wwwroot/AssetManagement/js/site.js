$(document).ready(function () {
    var data = $('#tabledataUser').DataTable({
        //"dom": 'Bfrtip',
        //"buttons": [
        //    'copy', 'csv', 'excel', 'pdf', 'print'
        //],
        "ajax": {
            "url": "https://localhost:44395/API/Accounts/UserData",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            { 'data': null },
            { 'data': 'name' },
            {
                'data': 'contact',
                render: function (data, type, row) {
                    return "+62" + data.slice(1)
                }
            },
            { 'data': 'department' },
            { 'data': 'role' },
            {
                'data': null,
                render: function (data, type, row, meta) {
                    return ' <button id="buttonDetail" type="button" class="btn btn-primary" data-toggle="modal" data-target="#modalDetailUser" data-toggle="tooltip" data-placement="top" title="Detail"><i class="fas fa-info-circle"></i></button> ' +
                        ' <button id="buttonUpdate" type="button" class="btn btn-warning" data-toggle="modal" data-toggle="tooltip" data-placement="top" title="Update Profile"><i class="fas fa-user-edit"></i></button> ' + ' <button id="buttonDelete" type="button" class="btn btn-danger" data-toggle="modal" data-toggle="tooltip" data-placement="top" title="Delete"><i class="fas fa-trash-alt"></i></button> '

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
                "targets": 5,
                 className: 'dt-body-center'
            }        ],
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

function AddNewUser(inputtxt,inputEmail) {
    var user = new Object();
    user.id = $('#id').val();
    user.firstName = $('#firstName').val();
    user.lastName = $('#lastName').val();
    user.genderId = $('#genderId').val();
    user.birthDate = $('#birthDate').val();
    user.address = $('#address').val();
    user.contact = $('#contact').val();
    user.departmentId = $('#departmentId').val();
    user.email = $('#email').val();
    user.password = $('#password').val();
    user.roleId = $('#roleId').val();
    var emaill = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    var passw = /^[A-Za-z\d]\w{8,}$/;
    if (inputEmail.value.match(emaill)) {
        if (inputtxt.value.match(passw)) {
            if (user.Id == "" || user.FirstName == "" || user.LastName == "" || user.GenderId == "" || user.BirthDate == "" || user.Address == "" || user.Contact == "" || user.DepartmentId == "" || user.Email == "" || user.Password == "" || user.RoleId == "") {
                Swal.fire('Error', 'Something Went Wrong', 'error');
            }
            else {
                $.ajax({
                    type: "POST",
                    url: 'https://localhost:44395/API/Users/Register',
                    data: JSON.stringify(user),
                    contentType: "application/json; charset=utf-8",
                    datatype: "json"
                }).done((result) => {
                    Swal.fire(
                        'Success',
                        'User Has been Added',
                        'success'
                    )
                    $('#tabledataUser').DataTable().ajax.reload();

                }).fail((error) => {
                    Swal.fire('Error', 'Something Went Wrong', 'error');
                });

            }
            
        }
        else {
            Swal.fire('Error', 'Something Went Wrong', 'error');
        }
        
    }
    else {
        Swal.fire('Error', 'Something Went Wrong', 'error');
    }
    
}

// Ini Detail User
$("#tabledataUser").on('click', '#buttonDetail', function () {
    var data = $("#tabledataUser").DataTable().row($(this).parents('tr')).data();
    console.log(data);
    $('#idD').val(data.id);
    $('#firstNameD').val(data.firstName);
    $('#lastNameD').val(data.lastName);
    $('#genderIdD').val(data.genderId);
    $('#birthDateD').val(data.birthDate.slice(0, 10));
    $('#addressD').val(data.address);
    $('#contactD').val(data.contact);
    $('#departmentIdD').val(data.departmentId);
    $('#emailD').val(data.email);
    $('#roleIdD').val(data.roleId);

});

// Ini Update User
$("#tabledataUser").on('click', '#buttonUpdate', function () {
    var data = $("#tabledataUser").DataTable().row($(this).parents('tr')).data();
    console.log(data);
    $('#idE').val(data.id);
    $('#firstNameE').val(data.firstName);
    $('#lastNameE').val(data.lastName);
    $('#genderIdE').val(data.genderId);
    $('#birthDateE').val(data.birthDate.slice(0, 10));
    $('#addressE').val(data.address);
    $('#contactE').val(data.contact);
    $('#departmentIdE').val(data.departmentId);
    $('#emailE').val(data.email);
    $('#roleIdE').val(data.roleId);

    $("#modalUpdateUser").modal("show");
    $("#modalUpdateUser").on('click', '#editUser', function () {

        var edit = new Object();
        edit.id = $('#idE').val();
        edit.firstName = $('#firstNameE').val();
        edit.lastName = $('#lastNameE').val();
        edit.genderId = $('#genderIdE').slice(0, 10).val();
        edit.birthDate = $('#birthDateE').val();
        edit.address = $('#addressE').val();
        edit.contact = $('#contactE').val();
        edit.email = $('#emailE').val();
        edit.departmentId = $('#departmentIdE').val();
        edit.isDeleted = 0;
        console.log(edit);
        console.log(edit.id);
        $.ajax({
            url: 'https://localhost:44395/API/Users',
            type: "PUT",
            data: JSON.stringify(edit),
            contentType: "application/json; charset=utf-8",
            datatype: "json"
        }).done((result) => {
            Swal.fire('Success', 'User Has been Updated', 'success');
            $('#tabledataUser').DataTable().ajax.reload();

        }).fail((error) => {
            Swal.fire('Error', 'Something Went Wrong', 'error');
        });
    })

});

// Ini Soft Delete User
$("#tabledataUser").on('click', '#buttonDelete', function () {
    var data = $("#tabledataUser").DataTable().row($(this).parents('tr')).data();
    console.log(data);
    $('#idE').val(data.id);
    $('#firstNameE').val(data.firstName);
    $('#lastNameE').val(data.lastName);
    $('#genderIdE').val(data.genderId);
    $('#birthDateE').val(data.birthDate.slice(0, 10));
    $('#addressE').val(data.address);
    $('#contactE').val(data.contact);
    $('#departmentIdE').val(data.departmentId);
    $('#emailE').val(data.email);
    $('#roleIdE').val(data.roleId);
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            var edit = new Object();
            edit.id = $('#idE').val();
            edit.firstName = $('#firstNameE').val();
            edit.lastName = $('#lastNameE').val();
            edit.genderId = $('#genderIdE').slice(0, 10).val();
            edit.birthDate = $('#birthDateE').val();
            edit.address = $('#addressE').val();
            edit.contact = $('#contactE').val();
            edit.email = $('#emailE').val();
            edit.departmentId = $('#departmentIdE').val();
            edit.isDeleted = 1;
            $.ajax({
                url: 'https://localhost:44395/API/Users',
                type: "PUT",
                data: JSON.stringify(edit),
                contentType: "application/json; charset=utf-8",
                datatype: "json"
            }).done((result) => {
                $('#tabledataUser').DataTable().ajax.reload();

            }).fail((error) => {
                Swal.fire('Error', 'Something Went Wrong', 'error')
            });
            Swal.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success'
            )
        }
    })

});