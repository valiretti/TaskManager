var projects = [];
var employees = [];

var statusStrings = ["Not Started", "In Progress", "Completed", "Postponed"];

function ConvertTimeSpanToHours(str) {
    let parts = str.split(":");
    return parseInt(parts[0], 10) + parseInt(parts[1], 10) / 60.0;
}

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


