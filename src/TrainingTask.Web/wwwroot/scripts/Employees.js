var idForEdit;

function GetEmployees() {
    $.ajax({
        url: '/api/employees',
        type: 'GET',
        contentType: "application/json",
        success: function (employees) {
            $.each(employees, function (index, employee) {
                $("table tbody").append(row(employee));
            });
        },
        error: function (jxqr, error, status) {
            errorHandling(jxqr, 0);
        }
    });
}

function GetEmployee(id) {
    $.ajax({
        url: '/api/employees/' + id,
        type: 'GET',
        contentType: "application/json",
        success: function (employee) {
            var form = document.forms["employeeForm"];
            form.elements["id"].value = employee.id;
            form.elements["firstName"].value = employee.firstName;
            form.elements["lastName"].value = employee.lastName;
            form.elements["patronymic"].value = employee.patronymic;
            form.elements["position"].value = employee.position;
        },
        error: function (jxqr, error, status) {
            errorHandling(jxqr, id);
        }
    });
}

function CreateEmployee(employee) {
    $.ajax({
        url: "api/employees",
        contentType: "application/json",
        method: "POST",
        data: GetJson(employee),
        success: function (empl) {
            closeForm();
            $("table tbody").append(row(empl));
        },

        error: function (jxqr, error, status) {
            errorHandling(jxqr, employee.id);
        }
    });
}

function GetJson(employee) {
    let data = JSON.stringify({
        Id: employee.id,
        FirstName: employee.firstName,
        LastName: employee.lastName,
        Patronymic: employee.patronymic,
        Position: employee.position
    });
    return data;
}

function EditEmployee(employee) {
    $.ajax({
        url: "api/employees",
        contentType: "application/json",
        method: "PUT",
        data: GetJson(employee),
        success: function () {
            closeForm();
            $("tr[data-rowid='" + idForEdit + "']").replaceWith(row(employee));
        },
        error: function (jxqr, error, status) {
            errorHandling(jxqr, employee.id);
        }
    });
}

function DeleteEmployee(id) {
    $.ajax({
        url: "api/employees/" + id,
        contentType: "application/json",
        method: "DELETE",
        success: function () {
            closeForm();
            closeErrors();
            $("tr[data-rowid='" + id + "']").remove();
        },
        error: function (jxqr, error, status) {
            errorHandling(jxqr, id);
        }
    });
}

var row = function (employee) {
    let tr = $('<tr>').attr("data-rowid", employee.id).append(
        $('<td>').text(employee.id),
        $('<td>').text(employee.firstName),
        $('<td>').text(employee.lastName),
        $('<td>').text(employee.patronymic),
        $('<td>').text(employee.position),
        $('<td>').append(
            $('<a>').addClass("editLink").attr("data-id", employee.id).text("Edit |"),
            $('<a>').addClass("removeLink").attr("data-id", employee.id).text("Delete")));
    return tr;
};

function GetEmployeeFromForm() {
    let searchForm = document.forms["employeeForm"];
    let employee = {
        id: idForEdit,
        firstName: searchForm.elements["firstName"].value,
        lastName: searchForm.elements["lastName"].value,
        patronymic: searchForm.elements["patronymic"].value,
        position: searchForm.elements["position"].value
    };
    return employee;
}

function closeForm() {
    $('#employeeForm').hide();
    $('#employeeForm')[0].reset();
}

function closeErrors() {
    $('#errors').empty();
    $('#errors').hide();
}

function errorHandling(jxqr, id) {
    if (jxqr.status == 404) {
        $('#errors').append("<p>" + "The employee not found" + "</p>");
        closeForm();
        $("tr[data-rowid='" + id + "']").remove();
    }
    else if (jxqr.status == 500) {
        $('#errors').append(jxqr.responseText);
        closeForm();
    }
    else if (jxqr.responseText === "") {
        $('#errors').append("<p>" + jxqr.statusText + "</p>");
    }
    else {
        var response = JSON.parse(jxqr.responseText);
        if (response['']) {

            $.each(response[''], function (index, item) {
                $('#errors').append("<p>" + item + "</p>");
            });
        }
    }
    $('#errors').show();
}

$(function () {

    closeForm();

    $("#add").click(function (e) {
        e.preventDefault();
        $('#headerEdit').hide();
        $('#headerCreate').show();
        $('#employeeForm')[0].reset();
        $('#employeeForm').show();
        $('#edit').hide();
        $('#addButton').show();
        closeErrors();
    });

    $("#back").click(function (e) {
        e.preventDefault();
        closeForm();
    });

    $("#edit").click(function (e) {
        e.preventDefault();
        let employee = GetEmployeeFromForm();
        EditEmployee(employee);
    });

    $("form").submit(function (e) {
        e.preventDefault();
        closeErrors();
        let employee = GetEmployeeFromForm();
        CreateEmployee(employee);
    });

    $("body").on("click",
        ".editLink",
        function () {
            closeErrors();
            var id = $(this).data("id");
            $('#employeeForm').show();
            $('#edit').show();
            $('#headerEdit').show();
            $('#headerCreate').hide();
            $('#addButton').hide();
            idForEdit = id;
            GetEmployee(id);
        });

    $("body").on("click",
        ".removeLink",
        function () {
            var id = $(this).data("id");
            closeErrors();
            let isDelete = confirm("Are you sure to delete this employee?");
            if (isDelete) {
                DeleteEmployee(id);
            }
        });

    GetEmployees();
});