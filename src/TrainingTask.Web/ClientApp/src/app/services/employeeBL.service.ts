import {Injectable} from '@angular/core';
import {Employee} from '../models/employee';
import {EmployeeService} from './employee.service';
import {MatTable} from '@angular/material';
import _ from 'lodash';

@Injectable({
  providedIn: 'root',
})
export class EmployeeBLService {

  constructor(private employeeService: EmployeeService) {
  }

  saveEmployee(newEmployee: Employee, employees: Employee[], table: MatTable<any>) {
    !!newEmployee.id ?
      this.editEmployee(newEmployee, employees, table) :
      this.addEmployee(newEmployee, employees, table);
  }

  addEmployee(newEmployee: Employee, employees: Employee[], table: MatTable<any>) {
    this.employeeService.createEmployee(newEmployee)
      .subscribe((employee: Employee) => {
        employees.push(employee);
        table.renderRows();
      });
  }

  editEmployee(changeEmployee: Employee, employees: Employee[], table: MatTable<any>) {
    this.employeeService.updateEmployee(changeEmployee)
      .subscribe(() => {
        const index = _.findIndex(employees, (employee: Employee) => employee.id === changeEmployee.id);
        employees.splice(index, 1, changeEmployee);
        table.renderRows();
      });
  }

  deleteEmployee(employeeId: number, employees: Employee[], table: MatTable<any>) {
    this.employeeService.deleteEmployee(employeeId)
      .subscribe(() => {
        // TODO: нужно использовать понятные названия переменных по коду
        const index = _.findIndex(employees, (employee: Employee) => employee.id === employeeId);
        employees.splice(index, 1);
        table.renderRows();
      });
  }
}
