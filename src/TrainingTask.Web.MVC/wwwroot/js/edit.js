
$(document).ready(function () {
    $("#taskF").submit(function (e) {
        e.preventDefault();
        let task = GetFromForm();

        let tasks = getFromLocalStorage();
        tasks.push(task);
        $.each(tasks, function (key, value) {
            $(addItem(value));
        });
        history.go(-1);
    });
});

function GetFromForm() {
    let searchForm = document.forms["taskF"];
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

function getFromLocalStorage() {
    let taskString = localStorage.getItem('task');
    let task = taskString ? JSON.parse(taskString) : [];
    return task;
}

function GetTask(tasks, id) {
    return tasks.find(t => t.id == id || t.tempId == id);
}