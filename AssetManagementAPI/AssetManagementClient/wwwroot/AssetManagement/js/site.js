$(document).ready(function () {
    var data = $('#tabledata').DataTable({
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
                    return ' <button class="btn btn-primary" type="button" onclick="GetData(' + "'" + row.nik + "'" + ')"><i class="fas fa-info-circle"></i></button> ' + ' <button class="btn btn-warning" data-toggle="modal" data-target="#editModal" type="button" onclick="GetData(' + "'" + row.nik + "'" + ')"><i class="fas fa-user-edit"></i></button> ' +
                        ' <button class="btn btn-danger" type="button" onclick="Delete(' + "'" + row.nik + "'" + ',' + "'" + row.educationId + "'" + ')"><i class="fas fa-trash"></i></button>'

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

function Insert() {
    var obj = new Object();
    obj.NIK = $("#nik").val();
    obj.FirstName = $("#firstName").val();
    obj.LastName = $("#lastName").val();
    obj.Phone = $("#phone").val();
    obj.BirthDate = $("#birthDate").val();
    obj.Salary = $("#salary").val();
    obj.Email = $("#email").val();
    obj.Password = $("#password").val();
    obj.Degree = $("#degree").val();
    obj.GPA = $("#gpa").val();
    obj.UniversityId = $("#universityId").val();
    var passw = /^[A-Za-z]\w{8,}$/;

    $.ajax({
        type: "POST",
        url: 'https://localhost:44320/API/Accounts/Register/',
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        datatype: "json"

    }).done((result) => {
        alert("Register Berhasil");
        $('#tabledata').DataTable().ajax.reload();
    }).fail((error) => {
        alert("Register Gagal");
    });

}

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

function Delete(nik, educationId) {
    console.log(nik)
    console.log(educationId)
    $.ajax({
        url: "https://localhost:44320/API/Persons/" + nik + "/",
        type: 'DELETE'

    }).done((result) => {
    }).fail((error) => {
        alert("Delete Gagal");
    })
    $.ajax({
        url: "https://localhost:44320/API/Educations/" + educationId + "/",
        type: 'DELETE'

    }).done((result) => {
        alert("Delete Berhasil");
        $('#tabledata').DataTable().ajax.reload();
    }).fail((error) => {
        alert("Delete Gagal");
    })
}

function Edit() {
    var obj = new Object();
    obj.NIK = $("#nikEdit").val();
    obj.FirstName = $("#firstNameEdit").val();
    obj.LastName = $("#lastNameEdit").val();
    obj.Phone = $("#phoneEdit").val();
    obj.BirthDate = $("#birthDateEdit").val();
    obj.Salary = $("#salaryEdit").val();
    $.ajax({
        type: "PUT",
        url: 'https://localhost:44320/API/Persons/',
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        datatype: "json"

    }).done((result) => {
        alert("Update Data Berhasil");
        $('#tabledata').DataTable().ajax.reload();
    }).fail((error) => {
        alert("Update Data Gagal");
    });
}

function GetData(nik) {
    $.ajax({
        url: "https://localhost:44320/API/Accounts/Profile/" + nik + "/"
    }).done((result) => {
        console.log(result);
        nik = `<label for="nik">NIK</label><input type="text" class="form-control" id="nikEdit" placeholder="Your NIK" value="${result[0].nik}" required>`
        firstName = `<label for="firstName">First Name</label><input type="text" class="form-control" id="firstNameEdit" placeholder="Your NIK" value="${result[0].firstName}" required>`
        lastName = `<label for="lastName">Last Name</label><input type="text" class="form-control" id="lastNameEdit" placeholder="Your NIK" value="${result[0].lastName}" required>`
        phone = `<label for="phone">Phone</label><input type="text" class="form-control" id="phoneEdit" placeholder="Your NIK" value="${result[0].phone}" required>`
        salary = `<label for="salary">Salary</label><input type="text" class="form-control" id="salaryEdit" placeholder="Your NIK" value="${result[0].salary}" required>`

        $(".nik").html(nik);
        $(".firstName").html(firstName);
        $(".lastName").html(lastName);
        $(".phone").html(phone);
        $(".salary").html(salary);
    }).fail((error) => {
        console.log(error);
    });
}