import {Component, OnInit, ViewChild} from '@angular/core';
import {Task} from '../../models/task';
import {TaskDataSource} from '../../services/task.datasource';
import {DialogService} from '../../services/dialog.service';
import {statusList} from '../../constants/statusList';
import {TaskService} from '../../services/task.service';
import {MessageService} from '../../services/message.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css'],
  providers: [TaskDataSource]
})
export class TasksComponent implements OnInit {
  displayedColumns: string[] =
    ['id', 'projectName', 'name', 'startDate', 'finishDate', 'employees', 'status', 'edit', 'delete'];

  statusList: Map<number, string> = statusList;

  constructor(
    public taskDataSource: TaskDataSource,
    public dialogService: DialogService,
    private taskService: TaskService,
    public messageService: MessageService,
  ) {
  }

  ngOnInit() {
    this.taskDataSource.reloadTask();
  }

  handleCloseDialog = (isDataChanged: boolean) => {
    if (isDataChanged) {
      this.taskDataSource.reloadTask();
    }
  };

  onOpenDetailsTaskClick(task: Task = new Task()) {
    this.dialogService
      .openDetailsTaskDialog(task)
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
