import {Injectable} from '@angular/core';
import {Employee} from '../models/employee';
import {EmployeeDialogComponent} from '../components/employee-dialog/employee-dialog.component';
import {cloneDeep} from 'lodash';
import {MatDialog, MatDialogConfig} from '@angular/material';
import {EmployeeDataSource} from './employee.datasource';
import {DeleteDialogComponent} from '../components/delete-dialog/delete-dialog.component';

@Injectable()
export class DialogService {

  constructor(
    public dialog: MatDialog,
    public dataSource: EmployeeDataSource,
  ) {
  }

  openDialog<T>(dialogComponent: any, config: MatDialogConfig<T>) {
    const dialogRef = this.dialog.open(dialogComponent, config);

    // TODO: Нужно обязательно указывать типы данных. Имена переменных должныотражать характер информации, которая в них сожержиться
    dialogRef.afterClosed().subscribe((isChangeData: boolean) => {
      if (isChangeData) {
        this.dataSource.reloadEmployee();
      }
    });
  }

  openDetailsEmployeeDialog(employee: Employee) {
    const config: MatDialogConfig<Employee> = {
      width: '500px',
      data: cloneDeep(employee),
      disableClose: true
    };

    this.openDialog(EmployeeDialogComponent, config);
  }

  openDeleteEmployeeDialog(employeeId: number) {
    const config: MatDialogConfig<number> = {
      data: employeeId
    };

    this.openDialog(DeleteDialogComponent, config);
  }
}
