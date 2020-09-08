$(document).ready(function () {
});

function Verify() {
    var check = validate();
    if (check == false) {
        return false;
    }
    var code = $('#Code').val();
    $.ajax({
        url: "/verify/verify/" + code,
        data: {code : code},
        cache: false,
        type: "POST",
        dataType: "JSON"
    }).then((result) => {
        if (result.StatusCode == 200) {
            window.location.href = "/";
        } else {
            alertify.error(result);
        }
    });
}


function validate() {
    var isValid = true;
    if ($('#Code').val().trim() == "") {
        $('#Code').css('border-color', 'Red');
        alertify.error('Code Cannot Empty');
        isValid = false;
    }
    else {
        $('#Code').css('border-color', 'lightgrey');
    }

    return isValid;
}