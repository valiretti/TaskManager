import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpService } from '../http.service';
import { Employee } from '../employee';
import { MatDialog, MatTable } from '@angular/material';
import { EmployeeDialogComponent } from '../employee-dialog/employee-dialog.component';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers: [HttpService]
})
export class EmployeesComponent implements OnInit {
  displayedColumns: string[] = ['id', 'firstName', 'lastName', 'patronymic', 'position', 'edit', 'delete'];
  @ViewChild('table') table: MatTable<any>;

  employees: Employee[] = [];
  employee: Employee = new Employee;

  constructor(
    public dialog: MatDialog,
    private httpService: HttpService
  ) { }

  ngOnInit() {
    this.httpService.getEmployees()
      .subscribe(data => this.employees = data);
  }

  openAddEmployeeDialog(): void {
    let dialogRef = this.dialog.open(EmployeeDialogComponent, {
      width: '500px',
      data: {
        title: 'Add Employee',
        employee: this.employee
      },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.addEmployee(result);
      }
    });
  }

  addEmployee(employee: Employee): void {
    this.httpService.addEmployee(employee)
      .subscribe(employee => {
        this.employees.push(employee);
        this.table.renderRows();
      });
  }

  openEditEmployeeDialog(employee: Employee): void {
    let dialogRef = this.dialog.open(EmployeeDialogComponent, {
      width: '500px',
      data: {
        title: 'Edit Employee',
        employee: { ...employee }
      },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.editEmployee(result);
      }
    });
  }

  editEmployee(employee: Employee): void {
    this.httpService.updateEmployee(employee)
      .subscribe(() => {
        this.employees = this.employees.map(e => {
          return (e.id !== employee.id) ? e : employee;
        });
        this.table.renderRows();
      });
  }

  onDeleteEmployeeClick(employeeId: number): void {
    this.httpService.deleteEmployee(employeeId)
      .subscribe(() => {
        this.employees = this.employees.filter(e => e.id !== employeeId);
        this.table.renderRows();
      });
  }
}