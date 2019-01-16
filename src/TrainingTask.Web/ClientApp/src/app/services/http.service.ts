import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
    providedIn: 'root',
})
// TODO: Сервис должен быть обязательно декомпозирован.
// По Сути в данный момент приложение должно содержать 4 сервиса отвечающие за вызов API:
// 1. Общий API сервис, который будет делать вызовы сервера (додлжен обрабатывать ошибки, заголовки и прочее_)
// 2. Сервисы работы с сущностями Employee, Project, Task.
export class HttpService {

    private handleError(error: HttpErrorResponse) {
        if (error.error instanceof ErrorEvent) {
            console.error('An error occurred:', error.error.message);
        } else {
            console.error(
                `Backend returned code ${error.status}, ` +
                `body was: ${error.error}`);
        }
        return throwError(
            'Something bad happened; please try again later.');
    };

    constructor(private http: HttpClient) { }

    get<T>(url: string): Observable<T> {
        return this.http.get<T>(url).pipe(
            catchError(this.handleError)
        )
    }

    post<T>(url: string, data: T): Observable<T> {
        return this.http.post<T>(url, data).pipe(
            catchError(this.handleError)
        );
    }

    put<T>(url: string, data: T): Observable<T> {
        return this.http.put<T>(url, data).pipe(
            catchError(this.handleError)
        );
    }

    delete(url: string): Observable<{}> {
        return this.http.delete(url).pipe(
            catchError(this.handleError)
        );
    }


    // convertTimeSpanToHours(str) {
    //     let parts = str.split(":");
    //     return parseInt(parts[0], 10) + parseInt(parts[1], 10) / 60.0;
    // }

    // getTasks(): Observable<Task[]> {
    //     let self = this;

    //     let result = this.http.get<Task[]>('api/tasks')
    //         .pipe(
    //             /* TODO: В общем случае сервисы не должны преобразовывать модели данных.
    //             Логику преобразование данных лучше держать либо в геттерах либо в пайпах
    //             */
    //             map(data => {
    //                 return data.map(function (task: any) {
    //                     let t = new Task();
    //                     t.id = task.id;
    //                     t.projectAbbreviation = task.projectAbbreviation;
    //                     t.name = task.name;
    //                     t.workHours = self.convertTimeSpanToHours(task.workHours);
    //                     t.startDate = task.startDate;
    //                     t.finishDate = task.finishDate;
    //                     t.status = task.status;
    //                     t.projectId = task.projectId;
    //                     t.fullNames = task.fullNames;
    //                     t.employees = task.employees;
    //                     return t;
    //                 });
    //             }),
    //             catchError(err => throwError(err))
    //         )
    //     return result;
    // }

    // getTasksByProject(id: number): Observable<Task[]> {
    //     let self = this;

    //     return this.http.get<Task[]>(`api/projects/${id}/tasks`).pipe(
    //         map(data => {
    //             return data.map(function (task: any) {
    //                 let t = new Task();
    //                 t.id = task.id;
    //                 t.name = task.name;
    //                 t.workHours = self.convertTimeSpanToHours(task.workHours);
    //                 t.startDate = task.startDate;
    //                 t.finishDate = task.finishDate;
    //                 t.status = task.status;
    //                 t.projectId = task.projectId;
    //                 t.projectAbbreviation = task.projectAbbreviation;
    //                 t.fullNames = task.fullNames;
    //                 t.employees = task.employees;
    //                 return t;
    //             });
    //         }),
    //         catchError(err => throwError(err))
    //     )
    // }

    // getTasksById(id: number): Observable<Task> {
    //     let self = this;

    //     return this.http.get<Task>(`api/tasks/${id}`).pipe(
    //         map(data => {
    //             let t = new Task();
    //             t.id = data.id;
    //             t.name = data.name;
    //             t.workHours = self.convertTimeSpanToHours(data.workHours);
    //             t.startDate = data.startDate;
    //             t.finishDate = data.finishDate;
    //             t.status = data.status;
    //             t.projectId = data.projectId;
    //             t.projectAbbreviation = data.projectAbbreviation;
    //             t.fullNames = data.fullNames;
    //             t.employees = data.employees;
    //             return t;
    //         }),
    //         catchError(err => throwError(err))
    //     )
    // }

    // addTask(task: Task): Observable<Task> {
    //     let self = this;

    //     return this.http.post<Task>('api/tasks', task).pipe(
    //         map(data => {
    //             let t = new Task();
    //             t.id = data.id;
    //             t.name = data.name;
    //             t.workHours = self.convertTimeSpanToHours(data.workHours);
    //             t.startDate = data.startDate;
    //             t.finishDate = data.finishDate;
    //             t.status = data.status;
    //             t.projectId = data.projectId;
    //             t.projectAbbreviation = data.projectAbbreviation;
    //             t.fullNames = data.fullNames;
    //             t.employees = data.employees;
    //             return t;
    //         }),
    //         catchError(err => throwError(err))
    //     );
    // }
}
