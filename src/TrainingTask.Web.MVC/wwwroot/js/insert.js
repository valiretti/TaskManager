var employees = [];
var idForEdit;
var nextId = 0;

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

function GetEmployeeListItems(employees) {
    let li = [];
    $.each(employees, function (key, value) {
        li.push($("<li>").text(value));
    });
    return li;
}

function InsertTask(task) {
    $.extend(task,
        {
            fullNames: employees.filter(e => task.employees && task.employees.some(id => id == e.id))
                .map(e => e.firstName + " " + e.lastName + " " + e.patronymic)
        });
    $("table#tasks tbody").append(row(task));
}

function DeleteTask(id) {
    let tasks = getFromLocalStorage();
    let index = tasks.indexOf(GetTask(tasks, id));
    tasks.splice(index, 1);
    localStorage.removeItem("task");
    $.each(tasks, function (key, value) {
        $(addItem(value));
    });

    $("table#tasks tr[data-rowid='" + id + "']").remove();
}

function getFromLocalStorage() {
    let taskString = localStorage.getItem('task');
    let task = taskString ? JSON.parse(taskString) : [];
    return task;
}

function GetTask(tasks, id) {
    return tasks.find(t => t.id == id || t.tempId == id);
}


function EditTask(id) {
    let tasks = getFromLocalStorage();
    let task = GetTask(tasks, id);
    let index = tasks.indexOf(task);
    tasks.splice(index, 1);
    localStorage.removeItem("task");
    $.each(tasks, function (key, value) {
        $(addItem(value));
    });

    task = JSON.stringify(task);
    document.location.href = "/project/EditTask?json=" + task;

    //$.extend(task,
    //    {
    //        fullNames: employees.filter(e => task.employees && task.employees.some(id => id == e.id))
    //            .map(e => e.firstName + " " + e.lastName + " " + e.patronymic)
    //    });
    //$("table#tasks tr[data-rowid='" + idForEdit + "']").replaceWith(row(task));
}


function FillTask(task) {
    var form = document.forms["taskForm"];
    form.elements["id"].value = task.id;
    form.elements["name"].value = task.name;
    form.elements["workTime"].value = task.workHours;
    form.elements["startDate"].valueAsDate = new Date(task.startDate + "Z");
    form.elements["finishDate"].valueAsDate = new Date(task.finishDate + "Z");
    $("#status").val(task.status);
    $("#employees").val(task.employeeIds ? task.employeeIds : task.employees);
}


function insert() {
    let tasks = localStorage['task'];
    if (tasks != undefined) {
        let task = JSON.parse(tasks);
        localStorage.removeItem("task");
        $.each(task, function (key, value) {
            value.tempId = --nextId;
            addItem(value);
            $(InsertTask(value));
        });

    }
}

$(document).ready(function () {

    let employeesPromise = GetEmployees();
    FillEmployees(employees);

    insert();

    $("#createProj").click(function (e) {
        sendJson();
        localStorage.removeItem("task");
    });

    $("body").on("click",
        ".editLink",
        function () {
            var id = $(this).data("id");
            EditTask(id);
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
});

function sendJson(parameters) {
    document.getElementById("Tasks").value = localStorage.getItem("task");
}




function FillEmployees(employees) {
    $.each(employees, function (key, value) {
        $('#employees').append($("<option>").attr("value", value.id).text(value.firstName + " " + value.lastName + " " + value.patronymic));
    });
}

function GetEmployees() {
    var request = new XMLHttpRequest;
    request.onreadystatechange = reqReadyStateChange;
    function reqReadyStateChange() {
        if (request.readyState === 4) {
            //var status = request.status;
            //if (status === 200) {
            employees = JSON.parse(request.responseText);
            //}
        }
    }
    request.open("GET", "/project/GetEmployees");
    request.send();
}

