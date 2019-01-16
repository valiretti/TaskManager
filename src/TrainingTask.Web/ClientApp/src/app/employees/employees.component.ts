import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpService } from '../http.service';
import { Employee } from '../employee';
import {MatDialog, MatDialogConfig, MatTable} from "@angular/material";
import { EmployeeDialogComponent } from '../employee-dialog/employee-dialog.component';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  /*
   TODO: В реальных приложениях HttpService никогда не используется напрямую,
   В таком контексте использования сам по себе HttpService не будет являться синглтоном что может привести к неожиданным
   результаттам работы приложенияб когда будет добавлена обработка заголовков или ошибок сервера
  */
  providers: [HttpService]
})
// TODO: Компонента перегружена логикой.
// TODO: Всю логику, которая не имеет непосредственного отношения к шаблону нужно выносить в сервисы
// TODO: Обязательно нужно декомпозировать элементы приложения
export class EmployeesComponent implements OnInit {
  displayedColumns: string[] = ['id', 'firstName', 'lastName', 'patronymic', 'position', 'edit', 'delete'];
  // TODO: В контексте данной компоненты можно обойтись без ссылки "table" и использовать класс MatTable
  @ViewChild('table') table: MatTable<any>;

  // TODO: Стоит сделать это поле асинхронным (в идеале это должен быть класс расширяющий класс DataSource)
  employees: Employee[] = [];

  // TODO: В этом месте нет необходимости создавать новый объект
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
      // TODO: в этом месте в открытый диалог передаётся анонимный объект, что затрудняет типизацию!
      data: {
        title: 'Add Employee',
        employee: this.employee
      },
      disableClose: true
    });

    // TODO: Нужно обязательно указывать типы данных. Имена переменных должныотражать характер информации, которая в них сожержиться
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
    /* TODO: по сути этот метод дублирует код метода openAddEmployeeDialog().
      Добавление новой записи можно рассматривать как частный случай редактирования
    */
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
        // TODO: нужно использовать понятные названия переменных по коду
        this.employees = this.employees.filter(e => e.id !== employeeId);
        this.table.renderRows();
      });
  }
}
