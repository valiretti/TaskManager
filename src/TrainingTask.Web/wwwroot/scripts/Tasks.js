var idForEdit;

var statusStrings = ["Not Started", "In Progress", "Completed", "Postponed"];

var projects = [];
var employees = [];

function ConvertTimeSpanToHours(str) {
    let parts = str.split(":");
    return parseInt(parts[0], 10) + parseInt(parts[1], 10) / 60.0;
}

function GetTasks() {
    $.ajax({
        url: "api/tasks",
        type: 'GET',
        contentType: "application/json",
        success: function (tasks) {
            $.each(tasks, function (index, task) {
                $("table tbody").append(row(task));
            });
        }
    });
}

function CreateTask(task) {
    $.ajax({
        url: "api/tasks",
        contentType: "application/json",
        method: "POST",
        data: GetJson(task),
        success: function (t) {
            closeForm();
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

function GetJson(task) {
    let data = JSON.stringify({
        Id: task.id,
        ProjectId: task.project,
        Name: task.name,
        WorkHours: task.workTime,
        StartDate: task.startDate,
        FinishDate: task.finishDate,
        Status: task.status,
        Employees: task.employees
    });
    return data;
}

function GetProjects() {
    return new Promise(
        function (resolve, reject) {
            $.ajax({
                url: 'api/projects',
                type: 'GET',
                contentType: "application/json",
                success: p => {
                    projects = p;
                    resolve(p);
                },
                error: (jxqr, error, status) => reject(jxqr)
            });
        });
}

function GetEmployees() {
    return new Promise(
        function (resolve, reject) {
            $.ajax({
                url: 'api/employees',
                type: 'GET',
                contentType: "application/json",
                success: e => {
                    employees = e;
                    resolve(e);
                },
                error: (jxqr, error, status) => reject(jxqr)
            });
        });
}

function EditTask(task) {
    $.ajax({
        url: "api/tasks",
        contentType: "application/json",
        method: "PUT",
        data: GetJson(task),
        success: function () {
            closeForm();
            $.extend(task,
                {
                    projectAbbreviation: projects.find(p => p.id == task.project).abbreviation,
                    fullNames: employees.filter(e => task.employees && task.employees.some(id => id == e.id))
                        .map(e => e.firstName + " " + e.lastName + " " + e.patronymic)
                });
            $("tr[data-rowid='" + idForEdit + "']").replaceWith(row(task));
        }
    });
}

var row = function (task) {
    let tr = $('<tr>').attr("data-rowid", task.id).append(
        $('<td>').text(task.id),
        $('<td>').text(task.projectAbbreviation),
        $('<td>').text(task.name),
        $('<td>').text(new Date(task.startDate).toLocaleDateString()),
        $('<td>').text(new Date(task.finishDate).toLocaleDateString()),
        $('<td>').append(
            $('<ul>').append(GetEmployeeListItems(task.fullNames))
        ),
        $('<td>').text(statusStrings[task.status]),
        $('<td>').append(
            $('<a>').addClass("editLink").attr("data-id", task.id).text("Edit |"),
            $('<a>').addClass("removeLink").attr("data-id", task.id).text("Delete")));
    return tr;
};

function GetEmployeeListItems(employees) {
    let li = [];
    $.each(employees, function (key, value) {
        li.push($("<li>").text(value));
    });
    return li;
}

function FillProjects(projects) {
    $.each(projects, function (key, value) {
        $('#project').append($("<option>").attr("value", value.id).text(value.name));
    });
}

function FillEmployees(employees) {
    $.each(employees, function (key, value) {
        $('#employees').append($("<option>").attr("value", value.id).text(value.firstName + " " + value.lastName));
    });
}

function closeForm() {
    $('#taskForm').hide();
    $('#taskForm')[0].reset();
}

function FillTask(task) {
    var form = document.forms["taskForm"];
    form.elements["id"].value = task.id;
    $("#project").val(task.projectId);
    form.elements["name"].value = task.name;
    form.elements["workTime"].value = ConvertTimeSpanToHours(task.workHours);
    form.elements["startDate"].valueAsDate = new Date(task.startDate);
    form.elements["finishDate"].valueAsDate = new Date(task.finishDate);
    $("#status").val(task.status);
    $("#employees").val(task.employeeIds);
}

function GetTaskFromForm() {
    let searchForm = document.forms["taskForm"];
    let task = {
        id: idForEdit,
        project: searchForm.elements["project"].value,
        name: searchForm.elements["name"].value,
        workTime: searchForm.elements["workTime"].value,
        startDate: searchForm.elements["startDate"].value,
        finishDate: searchForm.elements["finishDate"].value,
        status: searchForm.elements["status"].value,
        employees: $("#employees").val()
    };
    return task;
}

function DeleteTask(id) {
    $.ajax({
        url: "api/tasks/" + id,
        contentType: "application/json",
        method: "DELETE",
        success: function () {
            $("tr[data-rowid='" + id + "']").remove();
        }
    });
}

function GetTask(id) {
    return new Promise(
        function (resolve, reject) {
            $.ajax({
                url: '/api/tasks/' + id,
                type: 'GET',
                contentType: "application/json",
                success: employees => resolve(employees),
                error: (jxqr, error, status) => reject(jxqr)
            });
        });
}

$(function () {

    closeForm();

    let projectsPromise = GetProjects();
    let employeesPromise = GetEmployees();

    let projectsUIPromise = projectsPromise
        .then(projects => FillProjects(projects));
    let employeesUIPromise = employeesPromise
        .then(employees => FillEmployees(employees));
    let uiPromise = Promise.all([projectsUIPromise, employeesUIPromise]);

    $("#add").click(function (e) {
        e.preventDefault();
        $('#headerEdit').hide();
        $('#headerCreate').show();
        $('#taskForm')[0].reset();
        $('#taskForm').show();
        $('#edit').hide();
        $('#addButton').show();
    });

    $("#back").click(function (e) {
        e.preventDefault();
        closeForm();
    });


    $("#edit").click(function (e) {
        e.preventDefault();
        let task = GetTaskFromForm();
        EditTask(task);
    });

    $("form").submit(function (e) {
        e.preventDefault();
        $('#errors').empty();
        $('#errors').hide();
        let task = GetTaskFromForm();
        CreateTask(task);
    });

    $("body").on("click",
        ".editLink",
        function () {
            var id = $(this).data("id");
            $('#taskForm').show();
            $('#edit').show();
            $('#headerEdit').show();
            $('#headerCreate').hide();
            $('#addButton').hide();
            idForEdit = id;
            uiPromise
                .then(() => GetTask(id))
                .then(task => FillTask(task));
        });

    $("body").on("click",
        ".removeLink",
        function () {
            var id = $(this).data("id");
            let isDelete = confirm("Are you sure to delete this task?");
            if (isDelete) {
                DeleteTask(id);
            }
        });

    GetTasks();
})