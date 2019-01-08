var idForEdit;
var idForEditProject;

var nextId = 0;
var allTasks = [];

function closeProjectForm() {
    $('#projectForm').hide();
    $('#projectForm')[0].reset();
}

function closeForm() {
    $('#taskForm').hide();
    $('#taskForm')[0].reset();
}

function hideButtonsProjectForm() {
    $('#projectButtons').hide();
}

function showButtonsProjectForm() {
    $('#projectButtons').show();
}

function showButtonsEditProjectForm() {
    $('#addButton').hide();
    $('#editProj').show();
    $('#backProj').show();
}

function closeErrors() {
    $('#errors').empty();
    $('#errors').hide();
}

function errorHandling(jxqr, id) {
    if (jxqr.status == 404) {
        $('#errors').append("<p>" + "The project not found" + "</p>");
        closeForm();
        closeProjectForm();
        $("tr[data-rowid='" + id + "']").remove();
    }
    else if (jxqr.status == 500) {
        $('#errors').append(jxqr.responseText);
        closeForm();
        closeProjectForm();
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

var row = function (task) {
    let tr = $('<tr>').attr("data-rowid", task.id || task.tempId).append(
        $('<td>').text(task.id > 0 ? task.id : undefined),
        $('<td>').text(task.name),
        $('<td>').text(new Date(task.startDate).toLocaleDateString()),
        $('<td>').text(new Date(task.finishDate).toLocaleDateString()),
        $('<td>').append(
            $('<ul>').append(GetEmployeeListItems(task.fullNames))
        ),
        $('<td>').text(statusStrings[task.status]),
        $('<td>').append(
            $('<a>').addClass("editLink").attr("data-id", task.id || task.tempId).text("Edit |"),
            $('<a>').addClass("removeLink").attr("data-id", task.id || task.tempId).text("Delete")));
    return tr;
};

var rowProj = function (project) {
    let tr = $('<tr>').attr("data-rowid", project.id).append(
        $('<td>').text(project.id),
        $('<td>').text(project.name),
        $('<td>').text(project.abbreviation),
        $('<td>').text(project.description),
        $('<td>').append(
            $('<a>').addClass("editProjLink").attr("data-id", project.id).text("Edit |"),
            $('<a>').addClass("removeProjLink").attr("data-id", project.id).text("Delete")));
    return tr;
};

function FillTask(task) {
    var form = document.forms["taskForm"];
    form.elements["id"].value = task.id;
    form.elements["name"].value = task.name;
    form.elements["workTime"].value = task.workHours;
    form.elements["startDate"].valueAsDate = new Date(task.startDate);
    form.elements["finishDate"].valueAsDate = new Date(task.finishDate);
    $("#status").val(task.status);
    $("#employees").val(task.employeeIds ? task.employeeIds : task.employees);
}

function InsertTask(task) {
    closeForm();
    allTasks.push(task);
    $.extend(task,
        {
            fullNames: employees.filter(e => task.employees && task.employees.some(id => id == e.id))
                .map(e => e.firstName + " " + e.lastName + " " + e.patronymic)
        });
    $("table#tasks tbody").append(row(task));
}

function EditTask(task) {
    closeForm();
    let index = allTasks.indexOf(GetTask(idForEdit));
    allTasks[index] = task;
    $.extend(task,
        {
            fullNames: employees.filter(e => task.employees && task.employees.some(id => id == e.id))
                .map(e => e.firstName + " " + e.lastName + " " + e.patronymic)
        });
    $("table#tasks tr[data-rowid='" + idForEdit + "']").replaceWith(row(task));
}

function DeleteTask(id) {
    let index = allTasks.indexOf(GetTask(id));
    allTasks.splice(index, 1);
    $("table#tasks tr[data-rowid='" + id + "']").remove();
}

function GetTask(id) {
    return allTasks.find(t => t.id == id || t.tempId == id);
}

function GetProjectFromForm() {
    let searchForm = document.forms["projectForm"];
    let project = {
        id: idForEditProject,
        name: searchForm.elements["name"].value,
        abbreviation: searchForm.elements["abbreviation"].value,
        description: searchForm.elements["description"].value,
        tasks: allTasks
    };
    return project;
}

function GetTaskFromForm() {
    let searchForm = document.forms["taskForm"];
    let task = {
        id: idForEdit,
        name: searchForm.elements["name"].value,
        workHours: searchForm.elements["workTime"].value,
        startDate: searchForm.elements["startDate"].value,
        finishDate: searchForm.elements["finishDate"].value,
        status: searchForm.elements["status"].value,
        employees: $("#employees").val()
    };
    return task;
}

function GetJson(project) {
    let data = JSON.stringify({
        Id: project.id,
        Name: project.name,
        Abbreviation: project.abbreviation,
        Description: project.description,
        Tasks: project.tasks
    });
    return data;
}

function CreateProject(project) {
    $.ajax({
        url: "api/projects",
        contentType: "application/json",
        method: "POST",
        data: GetJson(project),
        success: function (t) {
            closeProjectForm();
            $("table#projects tbody").append(rowProj(t));
        },

        error: function (jxqr, error, status) {
            errorHandling(jxqr, project.id);
        }
    });
}

function GetAllProjects() {
    $.ajax({
        url: "api/projects",
        type: 'GET',
        contentType: "application/json",
        success: function (projects) {
            $.each(projects, function (index, project) {
                $("table#projects tbody").append(rowProj(project));
            });
        },
        error: function (jxqr, error, status) {
            errorHandling(jxqr, 0);
        }
    });
}

function DeleteProject(id) {
    $.ajax({
        url: "api/projects/" + id,
        contentType: "application/json",
        method: "DELETE",
        success: function () {
            $("table#projects tr[data-rowid='" + id + "']").remove();
        },

        error: function (jxqr, error, status) {
            errorHandling(jxqr, id);
        }
    });
}

function GetProject(id) {
    return new Promise(
        function (resolve, reject) {
            $.ajax({
                url: '/api/projects/' + id,
                type: 'GET',
                contentType: "application/json",
                success: projects => resolve(projects),
                error: (jxqr, error, status) => reject(jxqr)
            });
        });
}

function GetTasksByProject(id) {
    return new Promise(
        function (resolve, reject) {
            $.ajax({
                url: 'api/tasks/byProject/' + id,
                type: 'GET',
                contentType: "application/json",
                success: t => {
                    $.each(t, (i, e) => e.workHours = ConvertTimeSpanToHours(e.workHours));
                    allTasks = t;
                    resolve(t);
                },
                error: (jxqr, error, status) => reject(jxqr)
            });
        });
}

function FillProject(project) {
    var form = document.forms["projectForm"];
    form.elements["id"].value = project.id;
    form.elements["name"].value = project.name;
    form.elements["abbreviation"].value = project.abbreviation;
    form.elements["description"].value = project.description;
}

function FillTasks(tasks) {
    $.each(tasks, function (key, value) {
        $("table#tasks tbody").append(row(value));
    });
}

function EditProject(project) {
    $.ajax({
        url: "api/projects",
        contentType: "application/json",
        method: "PUT",
        data: GetJson(project),
        success: function () {
            closeProjectForm();
            $("table#projects tr[data-rowid='" + idForEditProject + "']").replaceWith(rowProj(project));
        },
        error: function (jxqr, error, status) {
            errorHandling(jxqr, project.id);
        }
    });
}

$(function () {

    closeForm();
    closeProjectForm();

    let projectsPromise = GetProjects();
    let employeesPromise = GetEmployees();

    let projectsUIPromise = projectsPromise
        .then(projects => FillProjects(projects));
    let employeesUIPromise = employeesPromise
        .then(employees => FillEmployees(employees));
    let uiPromise = Promise.all([projectsUIPromise, employeesUIPromise]);

    $("#addProject").click(function (e) {
        e.preventDefault();
        $('#headerEdit').hide();
        $('#headerCreate').show();
        $('#projectForm')[0].reset();
        $('#projectForm').show();
        $('#editProj').hide();
        $('#addButton').show();
        $("#tasks > tbody").empty();
        closeErrors();
    });

    $("#backProj").click(function (e) {
        e.preventDefault();
        closeProjectForm();
        closeErrors();
    });

    $("#editProj").click(function (e) {
        e.preventDefault();
        let project = GetProjectFromForm();
        EditProject(project);
    });

    $("#projectForm").submit(function (e) {
        e.preventDefault();
        $('#errors').empty();
        $('#errors').hide();
        let project = GetProjectFromForm();
        CreateProject(project);
        closeErrors();
    });

    $("#addTasks").click(function (e) {
        e.preventDefault();
        hideButtonsProjectForm();
        $('#headerEditTask').hide();
        $('#headerCreateTask').show();
        $('#taskForm')[0].reset();
        $('#taskForm').show();
        $('#editTask').hide();
        $('#addTaskButton').show();
    });

    $("#backTask").click(function (e) {
        e.preventDefault();
        closeForm();
        showButtonsProjectForm();
        $('#errors').empty();
        $('#errors').hide();
        closeErrors();
    });


    $("#editTask").click(function (e) {
        e.preventDefault();
        showButtonsProjectForm();
        let task = GetTaskFromForm();
        task.tempId = idForEdit;
        EditTask(task);
    });

    $("#taskForm").submit(function (e) {
        e.preventDefault();
        $('#errors').empty();
        $('#errors').hide();
        showButtonsProjectForm();

        let task = GetTaskFromForm();
        task.tempId = --nextId;
        task.Id = 0;
        InsertTask(task);
    });

    $("body").on("click",
        ".editProjLink",
        function () {
            var id = $(this).data("id");
            closeErrors();
            $("table#tasks tbody tr").remove();
            $('#projectForm').show();
            $('#editProj').show();
            $('#headerEdit').show();
            $('#headerCreate').hide();
            $('#addButton').hide();
            closeForm();
            idForEditProject = id;
            uiPromise
                .then(() => GetProject(id))
                .then(project => FillProject(project))
                .then(() => GetTasksByProject(id))
                .then(tasks => FillTasks(tasks))
                .catch(jxqr => errorHandling(jxqr, id));
        });

    $("body").on("click",
        ".removeProjLink",
        function () {
            var id = $(this).data("id");
            closeErrors();
            let isDelete = confirm("Are you sure to delete this project?");
            if (isDelete) {
                DeleteProject(id);
            }
        });

    $("body").on("click",
        ".editLink",
        function () {
            var id = $(this).data("id");
            hideButtonsProjectForm();
            $('#taskForm').show();
            $('#editTask').show();
            $('#headerEditTask').show();
            $('#headerCreateTask').hide();
            $('#addTaskButton').hide();
            idForEdit = id;
            FillTask(GetTask(id));
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

    GetAllProjects();
})