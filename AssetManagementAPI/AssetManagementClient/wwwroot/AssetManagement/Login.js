function Login() {
    var Login = new Object();
    Login.Email = $("#emailLogin").val();
    Login.Password = $("#passwordLogin").val();
    console.log(Login);
    $.ajax({
        type: 'post',
        url: '/Auth/Login',
        data: Login
    }).done((result) => {
        console.log("ok", result);
        if (result == '/Dashboard/Employee' || result == '/Dashboard/Manager' || result == '/Dashboard/Admin') {
            //alert("Successed to Login");
            localStorage.setItem('LoginRes', JSON.stringify(result));
            window.location.href = result;
            $("#emailLogin").val(null);
        }
        else {
            alert("Failed to Login");
            $("#emailLogin").val(null);
            $("#passwordLogin").val(null);
        }
    }).fail((result) => {
        console.log(result);
        alert("Failed to Login");
    })
}