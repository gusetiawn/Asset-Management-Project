function ResetPassword() {
    var ResetPass = new Object();
    ResetPass.email = $("#emailResetPass").val();
    console.log(ResetPass);
    $.ajax({
        type: "POST",
        url: "https://localhost:44395/API/Accounts/ForgotPassword/",
        data: JSON.stringify(ResetPass),
        contentType: "application/json; charset=utf-8",
        datatype: "json"
    }).done((result) => {
        Swal.fire(
            'Success',
            'Email has been sent. Please check your email!',
            'success'
        );
        $("#emailResetPass").val(null);
    }).fail((error) => {
        Swal.fire('Error', 'Something Went Wrong', 'error');
    });
}