$(document).ready(function () {
    loadData();
});

function loadData() {
    $.ajax({
        url: "/UserProfile/GetUser",
        data: "",
        cache: false,
        type: "GET",
        dataType: "JSON"
    }).then((result) => {
        if (result.Item2.StatusCode == 200) {
           // var RoleString = result.Item1.RoleName.Join();
            $('#Id').html(result.Item1.Id);
            $('#Email').html(result.Item1.Email);
            $('#UserName').html(result.Item1.UserName);
            $('#RoleName').html(result.Item1.RoleName.toString());
        }
    });
}