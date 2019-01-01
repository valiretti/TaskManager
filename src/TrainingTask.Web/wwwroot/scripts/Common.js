var projects = [];
var employees = [];

var statusStrings = ["Not Started", "In Progress", "Completed", "Postponed"];



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

function GetTaskFromForm() {
    let searchForm = document.forms["taskForm"];
    let task = {
        id: idForEdit,
        project: searchForm.elements["project"] ? searchForm.elements["project"].value : undefined,
        name: searchForm.elements["name"].value,
        workTime: searchForm.elements["workTime"].value,
        startDate: searchForm.elements["startDate"].value,
        finishDate: searchForm.elements["finishDate"].value,
        status: searchForm.elements["status"].value,
        employees: $("#employees").val()
    };
    return task;
}


