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
            //{ 'data': 'id' },
            { 'data': 'firstName' },
            { 'data': 'lastName' },
            //{ 'data': 'gender' },
            //{
            //    'data': 'birthDate',
            //    render: function (data, type, row) {
            //        return data.slice(0, 10)
            //    }
            //},
            //{ 'data': 'address' },
            {
                'data': 'contact',
                render: function (data, type, row) {
                    return "+62" + data.slice(1)
                }
            },
            //{ 'data': 'email' },
            { 'data': 'department' },
            { 'data': 'role' },
            {
                'data': null,
                render: function (data, type, row, meta) {
                    return ' <button id="buttonDetail" type="button" class="btn btn-primary" data-toggle="modal" data-target="#modalDetailUser"><i class="fas fa-info-circle"></i></button> ' +
                        ' <button id="buttonUpdate" type="button" class="btn btn-warning"><i class="fas fa-user-edit"></i></button> '


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

function AddNewUser() {
    var user = new Object();
    user.Id = $('#id').val();
    user.FirstName = $('#firstName').val();
    user.LastName = $('#lastName').val();
    user.GenderId = $('#genderId').val();
    user.BirthDate = $('#birthDate').val();
    user.Address = $('#address').val();
    user.Contact = $('#contact').val();
    user.DepartmentId = $('#departmentId').val();
    user.Email = $('#email').val();
    user.Password = $('#password').val();
    user.RoleId = $('#roleId').val();
    $.ajax({
        type: "POST",
        url: 'https://localhost:44395/API/Users/Register',
        data: JSON.stringify(user),
        contentType: "application/json; charset=utf-8",
        datatype: "json"
    }).done((result) => {
        $('#addNewUser').modal('hide');
        $('#tabledataUser').DataTable().ajax.reload();
        Swal.fire(
            'Success',
            'Item Has Been Added, Cek Your Email',
            'success'
        )

    }).fail((error) => {
        Swal.fire('Error', 'Something Went Wrong', 'error');
    });
}

//detail
$("#tabledataUser").on('click', '#buttonDetail', function () {
    var data = $("#tabledataUser").DataTable().row($(this).parents('tr')).data();
    console.log(data);
    $("#titleModal").text(data.firstName + " " + data.lastName);
    $('#modalDetailUser').find(".modal-body").html("<p>Id : " + data.id
        + "</p> <p>First Name : " + data.firstName
        + "</p> <p>Last Name  : " + data.lastName
        + "</p> <p>Gender       : " + data.gender
        + "</p> <p>Birth Date : " + data.birthDate
        + "</p> <p>Address     : " + data.address
        + "</p> <p>Contact     : " + data.contact
        + "</p> <p>Email      : " + data.email
        + "</p> <p>Department     : " + data.department
        + "</p> <p>Role    : " + data.role + "</p>");
});

//UPDATE
$("#tabledataUser").on('click', '#buttonUpdate', function () {
    var data = $("#tabledataUser").DataTable().row($(this).parents('tr')).data();
    console.log(data);
    $('#idE').val(data.id);
    $('#firstNameE').val(data.firstName);
    $('#lastNameE').val(data.lastName);
    $('#genderIdE').val(data.genderId);
    $('#birthDateE').val(data.birthDate.slice(0,10));
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
        edit.genderId = $('#genderIdE').slice(0,10).val();
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
            Swal.fire(
                'Success',
                'Item Has Been Added, Cek Your Email',
                'success'
            );
            $('#tabledataUser').DataTable().ajax.reload();

        }).fail((error) => {
            Swal.fire('Error', 'Something Went Wrong', 'error');
        });
    })
    
})