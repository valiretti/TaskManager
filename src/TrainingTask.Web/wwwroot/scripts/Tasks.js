var idForEdit;

function GetTasks() {
    $.ajax({
        url: "api/tasks",
        type: 'GET',
        contentType: "application/json",
        success: function (tasks) {
            $.each(tasks, function (index, task) {
                $("table tbody").append(row(task));
            });
        },

        error: function (jxqr, error, status) {
            errorHandling(jxqr, 0);
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

        error: function(jxqr, error, status) {
            errorHandling(jxqr, task.id);
        }
    });
}

function closeErrors() {
    $('#errors').empty();
    $('#errors').hide();
}

function errorHandling(jxqr, id) {
    if (jxqr.status == 404) {
        $('#errors').append("<p>" + "The task not found" + "</p>");
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
        },
        error: function (jxqr, error, status) {
            errorHandling(jxqr, task.id);
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
        },
        error: function (jxqr, error, status) {
            errorHandling(jxqr, id);
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
        closeErrors();
    });

    $("#back").click(function (e) {
        e.preventDefault();
        closeForm();
        closeErrors();
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
        closeErrors();
    });

    $("body").on("click",
        ".editLink",
        function () {
            var id = $(this).data("id");
            closeErrors();
            $('#taskForm').show();
            $('#edit').show();
            $('#headerEdit').show();
            $('#headerCreate').hide();
            $('#addButton').hide();
            idForEdit = id;
            uiPromise
                .then(() => GetTask(id))
                .then(task => FillTask(task))
                .catch(jxqr => errorHandling(jxqr, id));
        });

    $("body").on("click",
        ".removeLink",
        function () {
            var id = $(this).data("id");
            closeErrors();
            let isDelete = confirm("Are you sure to delete this task?");
            if (isDelete) {
                DeleteTask(id);
            }
        });

    GetTasks();
})