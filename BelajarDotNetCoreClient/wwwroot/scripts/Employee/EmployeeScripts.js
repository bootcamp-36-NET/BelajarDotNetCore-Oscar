var table = null

$(document).ready(function () {
    loadData();
});

function loadData() {
    table = $("#dataTable").DataTable({
        ajax: {
            url: "/Employees/LoadEmployee",
            type: "GET",
            dataType: "Json",
            dataSrc: ""
        },
        columns: [
            { data: null },
            { data: 'FirstName' },
            { data: 'LastName' },
            {
                data: "JoinDate",
                render: function (jsonDate) {
                    var date = moment(jsonDate).format("DD MMMM YYYY hh:mm");
                    return date;
                }
            },
            {
                render: function (data, type, row, meta) {
                    return '<Button class="btn btn-danger" onclick="return Delete(' + meta.row + ')">Delete</button>'
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

function Delete(index) {
    var Id = table.row(index).data().Id;
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
                url: "/Employees/Delete/" + Id,
                type: "POST",
                data: { id: Id },
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