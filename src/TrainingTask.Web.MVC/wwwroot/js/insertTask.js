var employees = [];
var nextId = 0;
var statusStrings = ["Not Started", "In Progress", "Completed", "Postponed"];

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
    updateStorage(id);
    $("table#tasks tr[data-rowid='" + id + "']").remove();
}

function updateStorage(id) {
    let tasks = getFromStorage();
    let task = GetTask(tasks, id);
    let index = tasks.indexOf(task);
    tasks.splice(index, 1);
    sessionStorage.removeItem('task');
    $.each(tasks, function (key, value) {
        $(addItem(value));
    });

    return task;
}

function EditTask(id) {
    let task = updateStorage(id);
    setIdToStorage(id);
    task = JSON.stringify(task);
    document.location.href = "/project/EditTask?json=" + task;
}

function insert() {
    let tasks = sessionStorage.getItem('task');
    if (tasks != undefined) {
        let task = JSON.parse(tasks);
        sessionStorage.removeItem('task');
        $.each(task, function (key, value) {
            value.tempId = --nextId;
            addItem(value);
            $(InsertTask(value));
        });

    }
}

function setIdToStorage(id) {
    sessionStorage.setItem('id', JSON.stringify(id));
}

function sendJson() {
    document.getElementById("Tasks").value = sessionStorage.getItem('task');
}

function GetEmployees() {
    return new Promise(
        function (resolve, reject) {
            $.ajax({
                url: '/project/GetEmployees',
                type: 'GET',
                contentType: "application/json",
                success: data => {
                    employees = data;
                    resolve();
                },
                error: (jxqr, error, status) => reject(jxqr)
            });
        });
}

$(function () {
    GetEmployees()
        .then(() => insert());

    $("#createProj").click(function (e) {
        sendJson();
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



