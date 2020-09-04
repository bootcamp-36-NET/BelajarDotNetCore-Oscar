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
        if (result.StatusCode == 200) {
            window.location.href = "/";
        }
    });
}

function validate() {
    var isValid = true;
    if ($('#Email').val().trim() == "") {
        $('#Email').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Email').css('border-color', 'lightgrey');
    }
    if ($('#Password').val().trim() == "") {
        $('#Password').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Password').css('border-color', 'lightgrey');
    }
    return isValid;
}