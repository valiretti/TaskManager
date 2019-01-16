import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from './http.service';
import { Project } from '../models/project';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {

  private projectsUrl = 'api/projects';

  constructor(private httpService: HttpService) { }

  getProjects(): Observable<Project[]> {
    return this.httpService.get<Project[]>(this.projectsUrl);
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

}