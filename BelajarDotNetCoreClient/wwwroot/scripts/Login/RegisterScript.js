$(document).ready(function () {
});

function Register() {
    var check = validate();
    if (check == false) {
        return false;
    }
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
        if (result.Item2.StatusCode == 200) {
            window.location.href = "/login";
        } else {
            alertify.error(result.Item2);
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
    if ($('#ConfirmPassword').val().trim() == "") {
        $('#ConfirmPassword').css('border-color', 'Red');
        alertify.error('Confirm Password must be same !');
        isValid = false;
    }
    else if ($('#ConfirmPassword').val().trim() != $('#Password').val().trim()) {
        $('#ConfirmPassword').css('border-color', 'Red');
        alertify.error('Password and Confirm Password must be same !');
        isValid = false;
    }
    else {
        $('#ConfirmPassword').css('border-color', 'lightgrey');
    }

    return isValid;
}