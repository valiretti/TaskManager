function GetTaskFromForm() {
    let searchForm = document.forms["taskForm"];
    let task = {
        id: 0,
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

