$(document).ready(function () {
});

function Login() {
    var check = validate();
    if (check == false) {
        return false;
    }
    var loginViewModel = {
        Email: $('#Email').val(),
        Password: $('#Password').val()
    };
    $.ajax({
        url: "/Login/Validate",
        data: loginViewModel,
        cache: false,
        type: "POST",
        dataType: "JSON"
    }).then((result) => {
        if (result.Item2.StatusCode == 200) {
            if (!result.Item1.EmailConfirmed) {
                window.location.href = "/verify";
            } else {
                window.location.href = "/UserProfile";
            }
        } else {
            alertify.error(result.Item1);
        }    
    });
}

function validate() {
    var isValid = true;
    if ($('#Email').val().trim() == "") {
        $('#Email').css('border-color', 'Red');
        alertify.error('Email Cannot Empty');
        isValid = false;
    }
    else {
        $('#Email').css('border-color', 'lightgrey');
    }
    if ($('#Password').val().trim() == "") {
        $('#Password').css('border-color', 'Red');
        alertify.error('Password Cannot Empty');
        isValid = false;
    }
    else {
        $('#Password').css('border-color', 'lightgrey');
    }
    return isValid;
}