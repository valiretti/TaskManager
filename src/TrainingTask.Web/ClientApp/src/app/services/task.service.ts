import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {HttpService} from './http.service';
import {Task} from '../models/task';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class TaskService {

  private tasksUrl = 'api/tasks';

  constructor(private httpService: HttpService) {
  }

  getTasks(): Observable<Array<Task>> {
    return this.httpService.get<Array<Task>>(`${this.tasksUrl}/all`).pipe(
      map((taskList: Array<any>) => {
        return taskList.map((task: any) => {
          const partStr = task.workHours.split(':');
          task.workHours = parseInt(partStr[0], 10) + parseInt(partStr[1], 10) / 60.0;
          return task;
        });
      })
    );
  }

  createTask(task: Task): Observable<Task> {
    return this.httpService.post<Task>(this.tasksUrl, task);
  }

  updateTask(task: Task): Observable<Task> {
    return this.httpService.put<Task>(this.tasksUrl, task);
  }

  deleteTask(id: number): Observable<{}> {
    return this.httpService.delete(`${this.tasksUrl}/${id}`);
  }

  saveTask(task: Task): Observable<Task> {
    return (Boolean(task.id)) ? this.updateTask(task) : this.createTask(task);
  }

}
