import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {Employee} from '../models/employee';
import {HttpService} from './http.service';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {

  private employeesUrl = 'api/employees';

  constructor(private httpService: HttpService) {
  }

  getEmployees(): Observable<Employee[]> {
    return this.httpService.get<Employee[]>(this.employeesUrl);
  }

  createEmployee(employee: Employee): Observable<Employee> {
    return this.httpService.post<Employee>(this.employeesUrl, employee);
  }

  updateEmployee(employee: Employee): Observable<Employee> {
    return this.httpService.put<Employee>(this.employeesUrl, employee);
  }

  deleteEmployee(employeeId: number): Observable<{}> {
    return this.httpService.delete(`${this.employeesUrl}/${employeeId}`);
  }

  saveEmployee(employee: Employee): Observable<Employee> {
    return (Boolean(employee.id)) ? this.updateEmployee(employee) : this.createEmployee(employee);
  }

}
