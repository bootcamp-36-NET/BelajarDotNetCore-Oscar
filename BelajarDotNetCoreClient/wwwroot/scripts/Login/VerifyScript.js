$(document).ready(function () {
});

function Verify() {
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
        }
    });
}