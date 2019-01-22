import {Injectable} from '@angular/core';
import {Employee} from '../models/employee';
import {EmployeeDialogComponent} from '../components/employee-dialog/employee-dialog.component';
import {cloneDeep} from 'lodash';
import {MatDialog, MatDialogConfig, MatDialogRef} from '@angular/material';
import {DeleteDialogComponent} from '../components/delete-dialog/delete-dialog.component';
import {Task} from '../models/task';
import {TaskDialogComponent} from '../components/task-dialog/task-dialog.component';
import {Project} from '../models/project';
import {ProjectDialogComponent} from '../components/project-dialog/project-dialog.component';

@Injectable({
  providedIn: 'root',
})
export class DialogService {

  constructor(
    public dialog: MatDialog,
  ) {
  }

  openDialog<T>(dialogComponent: any, config: MatDialogConfig<T>): MatDialogRef<any, any> {
    const dialogRef = this.dialog.open(dialogComponent, config);
    return dialogRef;
  }

  openDetailsEmployeeDialog(employee: Employee): MatDialogRef<any, any> {
    const config: MatDialogConfig<Employee> = {
      width: '500px',
      data: cloneDeep(employee),
      disableClose: true
    };

    return this.openDialog(EmployeeDialogComponent, config);
  }

  openDetailsTaskDialog(task: Task): MatDialogRef<any, any> {
    const config: MatDialogConfig<Task> = {
      width: '500px',
      data: cloneDeep(task),
      disableClose: true
    };

    return this.openDialog(TaskDialogComponent, config);
  }

  openDetailsProjectDialog(project: Project): MatDialogRef<any, any> {
    const config: MatDialogConfig<Project> = {
      width: '1000px',
      data: cloneDeep(project),
      disableClose: true
    };

    return this.openDialog(ProjectDialogComponent, config);
  }

  openDeleteDialog(): MatDialogRef<any, any> {
    return this.dialog.open(DeleteDialogComponent);
  }
}
