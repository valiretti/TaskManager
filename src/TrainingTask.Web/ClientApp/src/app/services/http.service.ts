import {Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {catchError, map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
// TODO: Сервис должен быть обязательно декомпозирован.
// По Сути в данный момент приложение должно содержать 4 сервиса отвечающие за вызов API:
// 1. Общий API сервис, который будет делать вызовы сервера (додлжен обрабатывать ошибки, заголовки и прочее_)
// 2. Сервисы работы с сущностями Employee, Project, Task.
export class HttpService {

  private handleError(error: HttpErrorResponse): Observable<never> {
    if (error.error instanceof ErrorEvent) {
      console.error('An error occurred:', error.error.message);
    } else {
      console.error(`Server responded with code ${error.status}`);
    }
    return throwError(
      'Something bad happened; please try again later.');
  }

  constructor(private http: HttpClient) {
  }

  get<T>(url: string): Observable<T> {
    return this.http.get<T>(url).pipe(
      catchError(this.handleError)
    );
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
}
