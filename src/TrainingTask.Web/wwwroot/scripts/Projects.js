var idForEdit;
var idForEditProject;

var nextId = 0;
var tasks = [];

function closeProjectForm() {
    $('#projectForm').hide();
    $('#projectForm')[0].reset();
}

function closeForm() {
    $('#taskForm').hide();
    $('#taskForm')[0].reset();
}

var row = function (task) {
    let tr = $('<tr>').attr("data-rowid", task.id || task.tempId).append(
        $('<td>').text(task.id),
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

function FillTask(task) {
    var form = document.forms["taskForm"];
    form.elements["id"].value = task.id;
    form.elements["name"].value = task.name;
    form.elements["workTime"].value = task.workHours ? ConvertTimeSpanToHours(task.workHours) : task.workTime;
    form.elements["startDate"].valueAsDate = new Date(task.startDate);
    form.elements["finishDate"].valueAsDate = new Date(task.finishDate);
    $("#status").val(task.status);
    $("#employees").val(task.employeeIds);
}

function InsertTask(task) {
    closeForm();
    tasks.push(task);
    $.extend(task,
        {
            fullNames: employees.filter(e => task.employees && task.employees.some(id => id == e.id))
                .map(e => e.firstName + " " + e.lastName + " " + e.patronymic)
        });
    $("table#tasks tbody").append(row(task));
}

function GetTask(id) {
    return tasks.find(t => t.id == id || t.tempId == id);
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
    });

    $("#backProj").click(function (e) {
        e.preventDefault();
        closeProjectForm();
        closeForm();
    });


    $("#editProj").click(function (e) {
        e.preventDefault();

    });

    $("form").submit(function (e) {
        e.preventDefault();
        $('#errors').empty();
        $('#errors').hide();

    });



    $("#addTasks").click(function (e) {
        e.preventDefault();
        $('#headerEditTask').hide();
        $('#headerCreateTask').show();
        $('#taskForm')[0].reset();
        $('#taskForm').show();
        $('#editTask').hide();
        $('#addTask').show();

    });

    $("#backTask").click(function (e) {
        e.preventDefault();
        closeForm();
    });


    $("#editTask").click(function (e) {
        e.preventDefault();
        //let task = GetTaskFromForm();
        //EditTask(task);
    });

    $("#addTask").click(function (e) {
        e.preventDefault();
        $('#errors').empty();
        $('#errors').hide();

        let task = GetTaskFromForm();
        task.tempId = --nextId;
        InsertTask(task);
    });

    $("body").on("click",
        ".editLink",
        function () {
            var id = $(this).data("id");
            $('#taskForm').show();
            $('#editTask').show();
            $('#headerEditTask').show();
            $('#headerCreateTask').hide();
            $('#addTask').hide();
            FillTask(GetTask(id));
        });

    //  $("body").on("click",
    //      ".removeLink",
    //      function () {
    //          var id = $(this).data("id");
    //          let isDelete = confirm("Are you sure to delete this task?");
    //          if (isDelete) {
    //              DeleteTask(id);
    //          }
    //      });

})