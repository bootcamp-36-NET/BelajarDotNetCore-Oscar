$(document).ready(function () {
});

function Register() {
    var registerModel = {
        Email: $('#Email').val(),
        Password: $('#Password').val(),
        ConfirmPassword: $('#ConfirmPassword').val()
    }
    $.ajax({
        url: "/Register/Register",
        data: registerModel,
        type: "POST",
        dataType: "JSON"
    }).then((result) => {
        if (result.StatusCode == 200) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Data Succesfully Added !',
                showConfirmButton: false,
                timer: 1500,
            })
            window.location.href = "/login";
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
        }
    });
}