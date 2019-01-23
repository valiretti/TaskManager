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
    const dialogConfig: MatDialogConfig<Employee> = new MatDialogConfig();
    dialogConfig.width = '500px';
    dialogConfig.disableClose = true;
    dialogConfig.data = cloneDeep(employee);

    return this.openDialog(EmployeeDialogComponent, dialogConfig);
  }

  openDetailsTaskDialog(task: Task, isDisabledProjectName: boolean): MatDialogRef<any, any> {
    const dialogConfig: MatDialogConfig<any> = new MatDialogConfig();
    dialogConfig.width = '500px';
    dialogConfig.disableClose = true;
    dialogConfig.data = {
      isDisabledProjectName: isDisabledProjectName,
      task: cloneDeep(task)
    };

    return this.openDialog(TaskDialogComponent, dialogConfig);
  }

  openDetailsProjectDialog(project: Project): MatDialogRef<any, any> {
    const dialogConfig: MatDialogConfig<Project> = new MatDialogConfig();
    dialogConfig.width = '1000px';
    dialogConfig.disableClose = true;
    dialogConfig.data = cloneDeep(project);

    return this.openDialog(ProjectDialogComponent, dialogConfig);
  }

  openDeleteDialog(): MatDialogRef<any, any> {
    return this.dialog.open(DeleteDialogComponent);
  }
}
