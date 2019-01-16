
$(document).ready(function () {
    $("#taskF").submit(function (e) {
        e.preventDefault();
        updateStorage();
        history.go(-1);
    });

    $("#Back").click(function (e) {
        updateStorage();
        history.go(-1);
    });
});

function GetFromForm() {
    let searchForm = document.forms["taskF"];
    let task = {
        id: getIdFromStorage(),
        name: searchForm.elements["Name"].value,
        workHours: searchForm.elements["WorkHours"].value,
        startDate: searchForm.elements["StartDate"].value,
        finishDate: searchForm.elements["FinishDate"].value,
        status: searchForm.elements["Status"].value,
        employees: $("#employees").val()
    };
    return task;
}

function updateStorage() {
    let task = GetFromForm();
    let tasks = getFromStorage();
    sessionStorage.removeItem('task');
    tasks.push(task);
    $.each(tasks, function (key, value) {
        $(addItem(value));
    });
}


function getIdFromStorage() {
    let id = sessionStorage.getItem('id');
    sessionStorage.removeItem('id');
    return parseInt(id);
}