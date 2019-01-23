import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {Task} from '../../models/task';
import {TaskDataSource} from '../../services/task.datasource';
import {DialogService} from '../../services/dialog.service';
import {statusList} from '../../constants/statusList';
import {TaskService} from '../../services/task.service';
import {MessageService} from '../../services/message.service';
import {ProjectService} from '../../services/project.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css'],
  providers: [TaskDataSource]
})
export class TasksComponent implements OnInit {
  @Input() projectId: number;
  @Output() setTasksByProject = new EventEmitter<Array<Task>>();
  displayedColumns: string[];
  isDisabledProjectName: boolean;

  statusList: Map<number, string> = statusList;

  constructor(
    public taskDataSource: TaskDataSource,
    public dialogService: DialogService,
    private taskService: TaskService,
    private projectService: ProjectService,
    public messageService: MessageService,
  ) {
  }

  ngOnInit() {
    this.taskDataSource.reloadTask(this.projectId);
    this.displayedColumns = Boolean(this.projectId) ?
      ['id', 'name', 'startDate', 'finishDate', 'employees', 'status', 'edit', 'delete'] :
      ['id', 'projectName', 'name', 'startDate', 'finishDate', 'employees', 'status', 'edit', 'delete'];
    this.isDisabledProjectName = Boolean(this.projectId) ? true : false;
    this.reloadTasksByProject();
  }

  reloadTasksByProject() {
    if (Boolean(this.projectId)) {
      this.projectService.getTasksByProject(this.projectId).subscribe((response: Array<Task>) => {
        this.setTasksByProject.emit(response);
      });
    } else {
      this.setTasksByProject.emit([]);
    }
  }

  handleCloseDialog = (isDataChanged: boolean) => {
    if (isDataChanged) {
      this.taskDataSource.reloadTask(this.projectId);
      this.reloadTasksByProject();
    }
  };

  onOpenDetailsTaskClick(task: Task = new Task()) {
    if (Boolean(this.projectId) && !Boolean(task.projectId)) {
      task.projectId = this.projectId;
    }
    this.dialogService
      .openDetailsTaskDialog(task, this.isDisabledProjectName)
      .afterClosed()
      .subscribe(this.handleCloseDialog);
  }

  onDeleteTaskClick(taskId: number) {
    this.dialogService
      .openDeleteDialog()
      .afterClosed()
      .subscribe((isDelete) => {
        if (isDelete) {
          this.taskService.deleteTask(taskId).subscribe(() => {
              this.handleCloseDialog(true);
              this.messageService.openSnackBar('deleted successfully', 'Success!');
            },
            () => {
              this.messageService.openSnackBar('unable to delete', 'Error!');
            });
        }
      });
  }
}
