function ChangePassword() {
    $.ajax({
        url: "https://localhost:44395/API/Accounts/Profile/123452/"
    }).done((result) => {
        var pass = new Object();
        pass.email = `${result[0].email}`
        pass.oldPassword = $('#input-currentPass').val();
        pass.newPassword = $('#input-newPass').val();
        $.ajax({
            type: "PUT",
            url: 'https://localhost:44395/API/Accounts/ChangePassword',
            data: JSON.stringify(pass),
            contentType: "application/json; charset=utf-8",
            datatype: "json"
        }).done((result) => {
            Swal.fire(
                'Success',
                'Password Successfully Changed',
                'success'
            );
            $("#input-currentPass").val(null);
            $("#input-newPass").val(null);
            $("#input-newPassAgain").val(null);
        }).fail((error) => {
            Swal.fire('Error', 'Something Went Wrong', 'error');
        });
    }).fail((error) => {
        Swal.fire('Error', 'Something Went Wrong', 'error');
    });
}
