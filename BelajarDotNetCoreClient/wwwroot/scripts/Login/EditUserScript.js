$(document).ready(function () {
    loadData();
});

function loadData() {
    $.ajax({
        url: "/Login/GetEdit",
        data: "",
        cache: false,
        type: "GET",
        dataType: "JSON"
    }).then((result) => {
        if (result.Item2.StatusCode == 200) {
            $('#Username').val(result.Item1.UserName);
        }
    });
}

function Edit() {
    var check = validate();
    if (check == false) {
        return false;
    }
    var userEditViewModel = {
        UserName: $('#Username').val(),
        OldPassword: $('#OldPassword').val(),
        NewPassword: $('#NewPassword').val(),
        ConfirmNewPassword: $('#ConfirmNewPassword').val()
    };
    $.ajax({
        url: "/Login/Edit",
        data: userEditViewModel,
        cache: false,
        type: "POST",
        dataType: "JSON"
    }).then((result) => {
        if (result.Item2.StatusCode == 200) {
            window.location.href = "/";
        } else {
            alertify.error(result.Item1);
        }
    });
}

function validate() {
    var isValid = true;
    if ($('#Username').val().trim() == "") {
        $('#Username').css('border-color', 'Red');
        alertify.error('Username Cannot Empty');
        isValid = false;
    }
    else {
        $('#Username').css('border-color', 'lightgrey');
    }
    if ($('#Password').val().trim() == "") {
        $('#Password').css('border-color', 'Red');
        alertify.error('Username Cannot Empty');
        isValid = false;
    }
    else {
        $('#Password').css('border-color', 'lightgrey');
    }
    if ($('#NewPassword').val().trim() == "") {
        $('#NewPassword').css('border-color', 'Red');
        alertify.error('NewPassword Cannot Empty');
        isValid = false;
    }
    else {
        $('#NewPassword').css('border-color', 'lightgrey');
    }
    if ($('#ConfirmNewPassword').val().trim() == "") {
        $('#ConfirmNewPassword').css('border-color', 'Red');
        alertify.error('ConfirmNewPassword Cannot Empty');
        isValid = false;
    } else if ($('#ConfirmNewPassword').val().trim() != $('#Password').val().trim()) {
        $('#ConfirmNewPassword').css('border-color', 'Red');
        alertify.error('Password and Confirm New Password must be same !');
        isValid = false;
    }
    else {
        $('#ConfirmNewPassword').css('border-color', 'lightgrey');
    }
    return isValid;
}