import {DataSource} from '@angular/cdk/table';
import {CollectionViewer} from '@angular/cdk/collections';
import {BehaviorSubject, Observable} from 'rxjs';
import {Injectable} from '@angular/core';
import {Task} from '../models/task';
import {TaskService} from './task.service';
import {ProjectService} from './project.service';

@Injectable()
export class TaskDataSource extends DataSource<Task> {

  public loading = new BehaviorSubject<boolean>(false);

  private data: BehaviorSubject<Array<Task>> = new BehaviorSubject([]);

  constructor(
    private taskService: TaskService,
    private projectService: ProjectService,
  ) {
    super();
  }

  connect(collectionViewer: CollectionViewer): Observable<Array<Task> | ReadonlyArray<Task>> {
    return this.data.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer) {
    this.data.complete();
    this.loading.complete();
  }

  public reloadTask(projectId: number) {
    this.loading.next(true);
    if (Boolean(projectId)) {
      this.projectService.getTasksByProject(projectId).subscribe((response: Array<Task>) => {
        this.data.next(response);
        this.loading.next(false);
      });
    } else {
      this.taskService.getTasks().subscribe((response: Array<Task>) => {
        this.data.next(response);
        this.loading.next(false);
      });
    }
  }
}
