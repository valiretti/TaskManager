import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Employee } from '../models/employee';
import { Project } from '../models/project';
import { Task } from '../models/task';
import { Observable, throwError } from 'rxjs';
import { tap, map, catchError } from 'rxjs/operators';

@Injectable()
// TODO: Сервис должен быть обязательно декомпозирован.
// По Сути в данный момент приложение должно содержать 4 сервиса отвечающие за вызов API:
// 1. Общий API сервис, который будет делать вызовы сервера (додлжен обрабатывать ошибки, заголовки и прочее_)
// 2. Сервисы работы с сущностями Employee, Project, Task.
export class HttpService {

    constructor(private http: HttpClient) { }

    // GET employees from the server
    getEmployees(): Observable<Employee[]> {
        return this.http.get<Employee[]>('api/employees').pipe(
            catchError(err => throwError(err))
        )
    }

    // POST: add a new employee to the server
    addEmployee(employee: Employee): Observable<Employee> {
        return this.http.post<Employee>('api/employees', employee).pipe(
            catchError(err => throwError(err))
        );
    }

    // PUT: update the employee on the server.
    updateEmployee(employee: Employee): Observable<Employee> {
        return this.http.put<Employee>('api/employees', employee).pipe(
            catchError(err => throwError(err))
        );
    }

    // DELETE: delete the employee from the server
    deleteEmployee(id: number): Observable<{}> {
        return this.http.delete(`api/employees/${id}`).pipe(
            catchError(err => throwError(err))
        );
    }

    // GET projects from the server
    getProjects(): Observable<Project[]> {
        return this.http.get<Project[]>('api/projects').pipe(
            catchError(err => throwError(err))
        )
    }

    // POST: add a new project to the server
    addProject(project: Project): Observable<Project> {
        return this.http.post<Project>('api/projects', project).pipe(
            catchError(err => throwError(err))
        );
    }

    // PUT: update the project on the server
    updateProject(project: Project): Observable<Project> {
        return this.http.put<Project>('api/projects', project).pipe(
            catchError(err => throwError(err))
        );
    }

    // DELETE: delete the project from the server
    deleteProject(id: number): Observable<{}> {
        return this.http.delete(`api/projects/${id}`).pipe(
            catchError(err => throwError(err))
        );
    }

    convertTimeSpanToHours(str) {
        let parts = str.split(":");
        return parseInt(parts[0], 10) + parseInt(parts[1], 10) / 60.0;
    }

    // GET tasks from the server
    getTasks(): Observable<Task[]> {
        let self = this;

        return this.http.get<Task[]>('api/tasks').pipe(
          /* TODO: В общем случае сервисы не должны преобразовывать модели данных.
          Логику преобразование данных лучше держать либо в геттерах либо в пайпах
          */
            map(data => {
                return data.map(function (task: any) {
                    let t = new Task();
                    t.id = task.id;
                    t.projectAbbreviation = task.projectAbbreviation;
                    t.name = task.name;
                    t.workHours = self.convertTimeSpanToHours(task.workHours);
                    t.startDate = task.startDate;
                    t.finishDate = task.finishDate;
                    t.status = task.status;
                    t.projectId = task.projectId;
                    t.fullNames = task.fullNames;
                    t.employees = task.employees;
                    return t;
                });
            }),
            catchError(err => throwError(err))
        )
    }

    // GET tasks by projectId from the server
    getTasksByProject(id: number): Observable<Task[]> {
        let self = this;

        return this.http.get<Task[]>(`api/projects/${id}/tasks`).pipe(
            map(data => {
                return data.map(function (task: any) {
                    let t = new Task();
                    t.id = task.id;
                    t.name = task.name;
                    t.workHours = self.convertTimeSpanToHours(task.workHours);
                    t.startDate = task.startDate;
                    t.finishDate = task.finishDate;
                    t.status = task.status;
                    t.projectId = task.projectId;
                    t.projectAbbreviation = task.projectAbbreviation;
                    t.fullNames = task.fullNames;
                    t.employees = task.employees;
                    return t;
                });
            }),
            catchError(err => throwError(err))
        )
    }

    // GET task by Id from the server
    getTasksById(id: number): Observable<Task> {
        let self = this;

        return this.http.get<Task>(`api/tasks/${id}`).pipe(
            map(data => {
                let t = new Task();
                t.id = data.id;
                t.name = data.name;
                t.workHours = self.convertTimeSpanToHours(data.workHours);
                t.startDate = data.startDate;
                t.finishDate = data.finishDate;
                t.status = data.status;
                t.projectId = data.projectId;
                t.projectAbbreviation = data.projectAbbreviation;
                t.fullNames = data.fullNames;
                t.employees = data.employees;
                return t;
            }),
            catchError(err => throwError(err))
        )
    }

    // POST: add a new task to the server
    addTask(task: Task): Observable<Task> {
        let self = this;

        return this.http.post<Task>('api/tasks', task).pipe(
            map(data => {
                let t = new Task();
                t.id = data.id;
                t.name = data.name;
                t.workHours = self.convertTimeSpanToHours(data.workHours);
                t.startDate = data.startDate;
                t.finishDate = data.finishDate;
                t.status = data.status;
                t.projectId = data.projectId;
                t.projectAbbreviation = data.projectAbbreviation;
                t.fullNames = data.fullNames;
                t.employees = data.employees;
                return t;
            }),
            catchError(err => throwError(err))
        );
    }

    // PUT: update the project on the server
    updateTask(task: Task): Observable<Task> {
        return this.http.put<Task>('api/tasks', task).pipe(
            catchError(err => throwError(err))
        );
    }

    // DELETE: delete the task from the server
    deleteTask(id: number): Observable<{}> {
        return this.http.delete(`api/tasks/${id}`).pipe(
            catchError(err => throwError(err))
        );
    }
}
