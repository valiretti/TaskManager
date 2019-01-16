function setTaskData(task) {
    sessionStorage.setItem('task', JSON.stringify(task));
}

function addItem(taskForInsert) {
    let taskString = sessionStorage.getItem('task');
    let task = taskString ? JSON.parse(taskString) : [];
    task.push(taskForInsert);
    setTaskData(task);
}

function getFromStorage() {
    let taskString = sessionStorage.getItem('task');
    let tasks = taskString ? JSON.parse(taskString) : [];
    return tasks;
}

function GetTask(tasks, id) {
    return tasks.find(t => t.id == id || t.tempId == id);
}