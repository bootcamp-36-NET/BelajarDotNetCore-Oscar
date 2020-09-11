var table = null

$(document).ready(function () {
    loadData();
    getDepartmentDropdown();
    // getDepartmentFilter();
});

function loadData() {

    //$('#dataTable thead tr').clone(true).appendTo('#dataTable thead');
    //$('#dataTable thead tr:eq(1) th').each(function (i) {
    //    var title = $(this).text();
    //    $(this).html('<input type="text" placeholder="Search ' + title + '" />');

    //    $('input', this).on('keyup change', function () {
    //        if (table.column(i).search() !== this.value) {
    //            table
    //                .column(i)
    //                .search(this.value)
    //                .draw();
    //        }
    //    });
    //});

    table = $('#dataTable').DataTable({
        orderCellsTop: true,
        fixedHeader: true,
        ajax: {
            url: "/Divisions/LoadAllData",
            type: "GET",
            dataType: "Json",
            dataSrc: "",
        },
        columns: [
            { data: null },
            { data: 'Name' },
            {
                data: "Department.Name",
                render: function (data, type, row) {
                    return row.Department.Name
                },
            },
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
                data: null,
                render: function (data, type, row) {
                    return '<Button class="btn btn-warning" onclick="return GetById(' + row.Id + ')">Update</button>'
                        + '&nbsp;'
                        + '<Button class="btn btn-danger" onclick="return Delete(' + row.Id + ')">Delete</button>'
                },
                orderable: false,
                searchable: false
            }
        ],
        initComplete: function () {
            this.api().columns(2).every(function () {
                var column = this;
                var select = $('<select><option value="">Show All</option></select>')
                    .appendTo($(column.header()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });

                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        },
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



function getDepartmentDropdown() {
    var departmentSelect = $('#Department');
    departmentSelect.empty();
    $.ajax({
        type: "GET",
        url: "/Departments/LoadDepartment",
        dataType: "Json",
        data: "",
        success: function (results) {
            if (results != null) {
                departmentSelect.append($('<option/>', {
                    value: "",
                    text: "Choose..."
                }));
                $.each(results, function (index, result) {
                    departmentSelect.append("<option value='" + result.Id + "'>" + result.Name + "</option>");
                });
            };
        },
        failure: function (response) {
            alert(response);
        }
    });
};

function getDepartmentFilter() {
    var departmentSelect = $('#DepartmentFilter');
    departmentSelect.empty();
    $.ajax({
        type: "GET",
        url: "/Departments/LoadDepartment",
        dataType: "Json",
        data: "",
        success: function (results) {
            if (results != null) {
                departmentSelect.append($('<option/>', {
                    value: "",
                    text: ""
                }));
                $.each(results, function (index, result) {
                    departmentSelect.append("<option value='" + result.Id + "'>" + result.Name + "</option>");
                });
            };
        },
        failure: function (response) {
            alert(response);
        }
    });
};

function Add() {
    var check = validate();
    if (check == false) {
        return false;
    }
    var division = {
        Name: $('#Name').val(),
        DepartmentId: $('#Department').val()
    };
    $.ajax({
        url: "/Divisions/AddOrUpdate",
        data: division,
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
                url: "/Divisions/Delete/" + Id,
                type: "POST",
                dataType: "JSON"
            }).then((result) => {
                if (result.StatusCode == 200) {
                    $('#myModal').modal('hide');
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: result.data,
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
        url: "/Divisions/GetById/",
        data: { Id: Id }
    }).then((result) => {
        $('#Id').val(result.Id);
        $('#Name').val(result.Name);
        $('#Department').val(result.DepartmentId);
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
    var division = {
        Id: $('#Id').val(),
        Name: $('#Name').val(),
        DepartmentId: $('#Department').val()
    };
    $.ajax({
        url: "/Divisions/AddOrUpdate",
        data: division,
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

function filterDepartment() {
    var selectedDepartment = $("DepartmentFilter").val();
    $.ajax({
        url: "/Divisions/FilteredData/" + selectedDepartment,
        data: "",
        type: "POST",
        dataType: "Json"
    }).then((result) => {
        if (result.StatusCode == 200) {
            table.loadData.
                table.ajax.reload(null, false);
        } else {
            Swal.fire('Error', 'Failed to Update', 'error');
            clearTextBox();
        }
    });
}

//Function for clearing the textboxes
function clearTextBox() {
    departmentSelect.empty();
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
    if ($('#Department').val().trim() == "") {
        $('#Department').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Department').css('border-color', 'lightgrey');
    }
    return isValid;
}

function printPdf() {
    $.ajax({
        url: "/Divisions/PrintPdf",
        data: "",
        type: "Get",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            var today = new Date();
            var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
            var sampleArr = base64ToArrayBuffer(response);
            saveByteArray("File-" + date, sampleArr);
        },
        failure: function (response) {
            alert("Cannot export thefile: Error in calling Ajax");
        }
    });
}

function printExcel() {
    $.ajax({
        url: "/Divisions/printExcel",
        data: "",
        type: "GET",
        dataType: "json",
        success: function (result) {
            Swal.fire('Success', 'Excel Downloaded', 'Success');
        }
    });
}

function saveByteArray(reportName, byte) {
    var blob = new Blob([byte]);
    var link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    var fileName = reportName + ".pdf";
    link.download = fileName;
    link.click();
}

function base64ToArrayBuffer(base64) {
    var binaryString = window.atob(base64);
    var binaryLen = binaryString.length;
    var bytes = new Uint8Array(binaryLen);
    for (var i = 0; i < binaryLen; i++) {
        var ascii = binaryString.charCodeAt(i);
        bytes[i] = ascii;
    }
    return bytes;
}
