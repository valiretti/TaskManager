import {Component, Inject, OnInit} from '@angular/core';
import {MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';
import {MessageService} from '../../services/message.service';
import {Task} from '../../models/task';
import {TaskService} from '../../services/task.service';
import {EmployeeDataSource} from '../../services/employee.datasource';
import {ProjectDataSource} from '../../services/project.datasource';
import {statusList} from '../../constants/statusList';

@Component({
  selector: 'app-task-dialog',
  templateUrl: './task-dialog.component.html'
})
export class TaskDialogComponent implements OnInit {

  statusList: Map<number, string> = statusList;

  constructor(
    private taskService: TaskService,
    private employeeDataSource: EmployeeDataSource,
    private projectDataSource: ProjectDataSource,
    public dialogRef: MatDialogRef<TaskDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Task,
    public messageService: MessageService,
  ) {
  }

  ngOnInit() {
    if (this.employeeDataSource.employeeList.length === 0) {
      this.employeeDataSource.reloadEmployee();
    }
    if (this.projectDataSource.projectList.length === 0) {
      this.projectDataSource.reloadProject();
    }
  }

  onSaveTaskClick(task: Task) {
    this.taskService.saveTask(task).subscribe(
      () => {
        this.closeDialog(true);
        this.messageService.openSnackBar('saved successfully', 'Success!');
      },
      () => {
        this.closeDialog(false);
        this.messageService.openSnackBar('unable to save', 'Error!');
      },
    );
  }

  onCancelClick() {
    this.closeDialog(false);
  }

  closeDialog(isChangeData: boolean) {
    this.dialogRef.close(isChangeData);
  }

}
