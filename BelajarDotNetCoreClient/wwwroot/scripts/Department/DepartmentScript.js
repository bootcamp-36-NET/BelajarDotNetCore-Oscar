var table = null

$(document).ready(function () {
    loadData();
});

function loadData() {
    table = $("#dataTable").DataTable({
        ajax: {
            url: "/Departments/LoadDepartment",
            type: "GET",
            dataType: "Json",
            dataSrc: ""
        },
        columns: [
            { data: null },
            { data: 'Name' },
            {
                data: "CreateDate",
                render: function (jsonDate) {
                    var date = moment(jsonDate).format("DD MMMM YYYY hh:mm");
                    return date;
                }
            },
            {
                data: "UpdateDate",
                render: function (jsonDate) {
                    if (!moment(jsonDate).isBefore('1000-01-01')) {
                        var date = moment(jsonDate).format("DD MMMM YYYY hh:mm");
                        return date;
                    }
                    return "Not Updated Yet";

                }
            },
            {
                data: 'Id',
                render: function (data, type, row) {
                    return '<Button class="btn btn-warning" onclick="return GetById(' + data + ')">Update</button>'
                        + '&nbsp;'
                        + '<Button class="btn btn-danger" onclick="return Delete(' + data + ')">Delete</button>'
                },
                orderable: false,
                searchable: false
            }
        ],
        "columnDefs": [{
            "searchable": false,
            "orderable": false,
            "targets": 0
        }],
        "order": [[1, 'asc']]
    });
    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
}

function Add() {
    var check = validate();
    if (check == false) {
        return false;
    }
    var department = {
        Name: $('#Name').val()
    };
    $.ajax({
        url: "/Departments/AddOrUpdate",
        data: department,
        type: "POST",
        dataType: "JSON"
    }).then((result) => {
        if (result.StatusCode == 200) {
            $('#myModal').modal('hide');
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Data Succesfully Added !',
                showConfirmButton: false,
                timer: 1500,
            })
            table.ajax.reload(null, false);
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            clearTextBox();
        }
    });
}

function Delete(Id) {
    Swal.fire({
        title: 'Confimation',
        text: "Are you sure want to delete this data",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Departments/Delete/" + Id,
                type: "POST",
                dataType: "JSON"
            }).then((result) => {
                if (result.StatusCode == 200) {
                    $('#myModal').modal('hide');
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'Data Succesfully Deleted !',
                        showConfirmButton: false,
                        timer: 1500,
                    })
                    table.ajax.reload(null, false);
                } else {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    clearTextBox();
                }
            });
        }
    })
}

function GetById(Id) {
    $.ajax({
        url: "/Departments/GetById/",
        data: { Id: Id }
    }).then((result) => {
        $('#Id').val(result.Id);
        $('#Name').val(result.Name);
        $('#btnAdd').hide();
        $('#btnUpdate').show();
        $('#myModal').modal('show');
    })
}

function Update() {
    var check = validate();
    if (check == false) {
        return false;
    }
    var department = {
        Id: $('#Id').val(),
        Name: $('#Name').val()
    };
    $.ajax({
        url: "/Departments/AddOrUpdate",
        data: department,
        type: "POST",
        dataType: "Json"
    }).then((result) => {
        if (result.StatusCode == 200) {
            $('#myModal').modal('hide');
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Data Succesfully Updated !',
                showConfirmButton: false,
                timer: 1500,
            })
            table.ajax.reload(null, false);
        } else {
            Swal.fire('Error', 'Failed to Update', 'error');
            clearTextBox();
        }
    });
}

//Function for clearing the textboxes
function clearTextBox() {
    $('#Name').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#Name').css('border-color', 'lightgrey');
}

function validate() {
    var isValid = true;
    if ($('#Name').val().trim() == "") {
        $('#Name').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }
    return isValid;
}