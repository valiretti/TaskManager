var idForEdit;
var statusStrings = ["Not Started", "In Progress", "Completed", "Postponed"];

function GetTaskFromForm() {
    let searchForm = document.forms["taskForm"];
    let task = {
        id: idForEdit,
        name: searchForm.elements["Name"].value,
        workHours: searchForm.elements["WorkHours"].value,
        startDate: searchForm.elements["StartDate"].value,
        finishDate: searchForm.elements["FinishDate"].value,
        status: searchForm.elements["Status"].value,
        employees: $("#employees").val()
    };
    return task;
}

$(document).ready(function () {
    $("#taskForm").submit(function (e) {
        e.preventDefault();
        let task = GetTaskFromForm();
        task.Id = 0;
        addItem(task);
        history.go(-1);
    });
});

function setTaskData(task) {
    localStorage.setItem('task', JSON.stringify(task));
}

function addItem(taskForInsert) {
    let taskString = localStorage.getItem('task');
    let task = taskString ? JSON.parse(taskString) : [];
    task.push(taskForInsert);
    setTaskData(task);
}