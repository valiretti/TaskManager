function CreateTask(task) {
    $.ajax({
        url: "api/tasks",
        contentType: "application/json",
        method: "POST",
        data: JSON.stringify({
            ProjectId: task.project,
            Name: task.firstName,
            WorkHours: task.lastName,
            StartDate: task.patronymic,
            FinishDate: task.position,
            Status: task.status,
            Employees: task.employees
        }),
        success: function (t) {
            var cancelButton = $("#back");
            cancelButton.click();
            $("table tbody").append(row(t));
        },

        error: function (jxqr, error, status) {
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
            }

            $('#errors').show();
        }
    });
}

var row = function (task) {
    return "<tr data-rowid='" + task.id + "'><td>" + task.id + "</td>" +
        "<td>" + task.project + "</td> <td>" + task.name + "</td> <td>" + task.startDate + "</td> <td>" + task.finishDate + "</td><td>" + "</td> <td>" + task.employees + "</td><td>"
        + "</td> <td>" + task.status + "</td>" + "<td><a class='editLink' data-id='" + task.id + "'>Edit</a> | " +
        "<a class='removeLink' data-id='" + task.id + "'>Delete</a></td></tr>";
};



$(function () {

    $("#add").click(function (e) {
        e.preventDefault();
        document.getElementById('headerEdit').setAttribute("style", "display:none;");
        document.getElementById('headerCreate').setAttribute("style", "display:block;");
        document.taskForm.reset();
        $('#taskForm').show();
    });

    $("#back").click(function (e) {
        e.preventDefault();
        $('#taskForm').hide();
        document.taskForm.reset();
    });


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
        };

       // EditEmployee(employee);
    });

    $("form").submit(function (e) {
        e.preventDefault();
        $('#errors').empty();
        $('#errors').hide();

        let tId = this.elements["id"].value;
        let tProject = this.elements["project"].value;
        let tName = this.elements["name"].value;
        let tWorkTime = this.elements["workTime"].value;
        let tStartDate = this.elements["startDate"].value;
        let tFinishDate = this.elements["finishDate"].value;
        let tStatus = this.elements["status"].value;
        let tEmployees = this.elements["employees"].value;

        let task = {
            id: tId,
            project: tProject,
            name: tName,
            workTime: tWorkTime,
            startDate: tStartDate,
            finishDate: tFinishDate,
            status: tStatus,
            employees: tEmployees
        };

        CreateTask(task);
    });

    $("body").on("click",
        ".editLink",
        function () {
            var id = $(this).data("id");
            document.getElementById('employeeForm').setAttribute("style", "display:block;");
            document.getElementById('edit').setAttribute("style", "display:inline;");
            document.getElementById('headerEdit').setAttribute("style", "display:block;");
            document.getElementById('headerCreate').setAttribute("style", "display:none;");
            $('#addButton').hide();
           // GetEmployee(id);
        });

    $("body").on("click",
        ".removeLink",
        function () {
            var id = $(this).data("id");
            let isDelete = confirm("Are you sure to delete this employee?");
            if (isDelete) {
               // DeleteEmployee(id);
            }
        });
})