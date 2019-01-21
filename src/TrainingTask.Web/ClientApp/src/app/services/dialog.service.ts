import {Injectable} from '@angular/core';
import {Employee} from '../models/employee';
import {EmployeeDialogComponent} from '../components/employee-dialog/employee-dialog.component';
import {cloneDeep} from 'lodash';
import {MatDialog, MatDialogConfig, MatDialogRef} from '@angular/material';
import {DeleteDialogComponent} from '../components/delete-dialog/delete-dialog.component';

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

  openDeleteEmployeeDialog(employeeId: number): MatDialogRef<any, any> {
    const config: MatDialogConfig<number> = {
      data: employeeId
    };

    return this.openDialog(DeleteDialogComponent, config);
  }
}
