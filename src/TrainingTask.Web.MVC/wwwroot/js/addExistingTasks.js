var tasks = [];

$(function () {
    $("body").on("click",
        ".editProjLink",
        function (e) {
            e.preventDefault();
            var id = $(this).data("id");
            GetTasksByProjectId(id)
                .then(() => addToStorage())
                .then(() => location.href = $(this).attr('href'));
        });
});


function addToStorage() {
    $.each(tasks, function (key, value) {
        $(addItem(value));
    });
}

function GetTasksByProjectId(id) {
    return new Promise(
        function (resolve, reject) {
            $.ajax({
                url: '/project/GetTasksByProjectId?id=' + id,
                type: 'GET',
                contentType: "application/json",
                success: data => {
                    tasks = data;
                    resolve();
                },
                error: (jxqr, error, status) => reject(jxqr)
            });
        });
}

