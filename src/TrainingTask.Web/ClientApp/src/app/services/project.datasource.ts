import {DataSource} from '@angular/cdk/table';
import {CollectionViewer} from '@angular/cdk/collections';
import {BehaviorSubject, Observable} from 'rxjs';
import {Injectable} from '@angular/core';
import {Project} from '../models/project';
import {ProjectService} from './project.service';

@Injectable()
export class ProjectDataSource extends DataSource<Project> {

  public loading = new BehaviorSubject<boolean>(false);

  private data: BehaviorSubject<Array<Project>> = new BehaviorSubject([]);

  get projectList(): Array<Project> {
    return this.data.value;
  }

  constructor(
    private projectService: ProjectService
  ) {
    super();
  }

  connect(collectionViewer: CollectionViewer): Observable<Array<Project> | ReadonlyArray<Project>> {
    return this.data.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer) {
    this.data.complete();
    this.loading.complete();
  }

  public reloadProject() {
    this.loading.next(true);
    this.projectService.getProjects().subscribe((response: Array<Project>) => {
      this.data.next(response);
      this.loading.next(false);
    });
  }
}
