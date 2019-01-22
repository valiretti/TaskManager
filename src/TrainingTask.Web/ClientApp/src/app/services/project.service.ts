import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {HttpService} from './http.service';
import {Project} from '../models/project';
import {Task} from '../models/task';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {

  private projectsUrl = 'api/projects';

  constructor(private httpService: HttpService) {
  }

  getProjects(): Observable<Project[]> {
    return this.httpService.get<Project[]>(`${this.projectsUrl}/all`);
  }

  getTasksByProject(id: number): Observable<Array<Task>> {
    return this.httpService.get<Array<Task>>(`${this.projectsUrl}/${id}/tasks`).pipe(
      map((taskList: Array<any>) => {
        return taskList.map((task: any) => {
          const partStr = task.workHours.split(':');
          task.workHours = parseInt(partStr[0], 10) + parseInt(partStr[1], 10) / 60.0;
          return task;
        });
      })
    );
  }

  createProject(project: Project): Observable<Project> {
    return this.httpService.post<Project>(this.projectsUrl, project);
  }

  updateProject(project: Project): Observable<Project> {
    return this.httpService.put<Project>(this.projectsUrl, project);
  }

  deleteProject(id: number): Observable<{}> {
    return this.httpService.delete(`${this.projectsUrl}/${id}`);
  }

  saveProject(project: Project): Observable<Project> {
    return (Boolean(project.id)) ? this.updateProject(project) : this.createProject(project);
  }

}
