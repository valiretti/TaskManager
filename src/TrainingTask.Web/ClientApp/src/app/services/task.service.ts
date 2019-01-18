import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from './http.service';
import { Task } from '../models/task';

@Injectable({
  providedIn: 'root',
})
export class TaskService {

  private tasksUrl = 'api/tasks';

  constructor(private httpService: HttpService) { }

  getTasks(): Observable<Task[]> {
    return this.httpService.get<Task[]>(this.tasksUrl);
  }

  getTasksByProject(id: number): Observable<Task[]> {
    return this.httpService.get<Task[]>(`api/projects/${id}/tasks`);
  }

  getTaskById(id: number): Observable<Task> {
    return this.httpService.get<Task>(`${this.tasksUrl}/${id}`);
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

}
