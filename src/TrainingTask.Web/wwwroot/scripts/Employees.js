var idForEdit;

function GetEmployees() {
    $.ajax({
        url: '/api/employees',
        type: 'GET',
        contentType: "application/json",
        success: function (employees) {
            var rows = "";
            $.each(employees, function (index, employee) {
                rows += row(employee);
            })
            $("table tbody").append(rows);
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
        }
    });
}

function CreateEmployee(employee) {
    $.ajax({
        url: "api/employees",
        contentType: "application/json",
        method: "POST",
        data: JSON.stringify({
            FirstName: employee.firstName,
            LastName: employee.lastName,
            Patronymic: employee.patronymic,
            Position: employee.position
        }),
        success: function (empl) {
            var cancelButton = $("#back");
            cancelButton.click();
            $("table tbody").append(row(empl));
        },

        error: function (jxqr, error, status) {
            console.log(jxqr);
            if (jxqr.responseText === "") {
                $('#errors').append("<h3>" + jxqr.statusText + "</h3>");
            }
            else {
                var response = JSON.parse(jxqr.responseText);
                if (response['']) {

                    $.each(response[''], function (index, item) {
                        $('#errors').append("<p>" + item + "</p>");
                    });
                }
                // добавляем ошибки свойства Name
                if (response['Name']) {

                    $.each(response['Name'], function (index, item) {
                        $('#errors').append("<p>" + item + "</p>");
                    });
                }

            }

            $('#errors').show();
        }
    })
}

function EditEmployee(employee) {
    $.ajax({
        url: "api/employees",
        contentType: "application/json",
        method: "PUT",
        data: JSON.stringify({
            Id: employee.id,
            FirstName: employee.firstName,
            LastName: employee.lastName,
            Patronymic: employee.patronymic,
            Position: employee.position
        }),
        success: function () {
            var cancelButton = $("#back");
            cancelButton.click();
            $("tr[data-rowid='" + idForEdit + "']").replaceWith(row(employee));
        }
    })
}

function DeleteEmployee(id) {
    $.ajax({
        url: "api/employees/" + id,
        contentType: "application/json",
        method: "DELETE",
        success: function () {
            $("tr[data-rowid='" + id + "']").remove();
        }
    })
}

var row = function (employee) {
    return "<tr data-rowid='" + employee.id + "'><td>" + employee.id + "</td>" +
        "<td>" + employee.firstName + "</td> <td>" + employee.lastName + "</td> <td>" + employee.patronymic + "</td> <td>" + employee.position + "</td>" +
        "<td><a class='editLink' data-id='" + employee.id + "'>Edit</a> | " +
        "<a class='removeLink' data-id='" + employee.id + "'>Delete</a></td></tr>";
}



$(function () {

    $("#add").click(function (e) {
        e.preventDefault();
        document.employeeForm.reset();
        $('#employeeForm').show();
    })

    $("#back").click(function (e) {
        e.preventDefault();
        $('#employeeForm').hide();
        document.employeeForm.reset();
    })


    $("#edit").click(function (e) {
        e.preventDefault();
      
        let searchForm = document.forms["employeeForm"];

        let eId = idForEdit;
        let eFirstName = searchForm.elements["firstName"].value;
        let eLastName = searchForm.elements["lastName"].value;
        let ePatronymic = searchForm.elements["patronymic"].value;
        let ePosition = searchForm.elements["position"].value;

        let employee = {
            id: eId,
            firstName: eFirstName,
            lastName: eLastName,
            patronymic: ePatronymic,
            position: ePosition
        }

        EditEmployee(employee);
    })

    $("form").submit(function (e) {
        e.preventDefault();
        $('#errors').empty();
        $('#errors').hide();

        let eId = this.elements["id"].value;
        let eFirstName = this.elements["firstName"].value;
        let eLastName = this.elements["lastName"].value;
        let ePatronymic = this.elements["patronymic"].value;
        let ePosition = this.elements["position"].value;

        let employee = {
            id: eId,
            firstName: eFirstName,
            lastName: eLastName,
            patronymic: ePatronymic,
            position: ePosition
        }

        CreateEmployee(employee);
    });

    $("body").on("click",
        ".editLink",
        function () {
            var id = $(this).data("id");
            document.getElementById('employeeForm').setAttribute("style", "display:block;");
            document.getElementById('edit').setAttribute("style", "display:inline;");
            $('#addButton').hide();
            idForEdit = id;
            GetEmployee(id);
        })

    $("body").on("click",
        ".removeLink",
        function () {
            var id = $(this).data("id");
            let isDelete = confirm("Are you sure to delete this employee?");
            if (isDelete) {
                DeleteEmployee(id);
            }
        })

    GetEmployees();
})