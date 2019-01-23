import {DataSource} from '@angular/cdk/table';
import {Employee} from '../models/employee';
import {CollectionViewer} from '@angular/cdk/collections';
import {BehaviorSubject, Observable, Subject} from 'rxjs';
import {Injectable} from '@angular/core';
import {EmployeeService} from './employee.service';

@Injectable()
export class EmployeeDataSource extends DataSource<Employee> {

  public loading = new BehaviorSubject<boolean>(false);
  public numberEmployees = new BehaviorSubject<number>(0);

  private data: BehaviorSubject<Array<Employee>> = new BehaviorSubject([]);

  get employeeList(): Array<Employee> {
    return this.data.value;
  }

  constructor(
    private employeeService: EmployeeService
  ) {
    super();
  }

  connect(collectionViewer: CollectionViewer): Observable<Array<Employee> | ReadonlyArray<Employee>> {
    return this.data.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer) {
    this.data.complete();
    this.loading.complete();
  }

  public reloadEmployee(page: number = 1, limit: number = 10) {
    this.loading.next(true);
    this.employeeService.getEmployees(page, limit).subscribe((response: any) => {
      this.data.next(response.items);
      this.loading.next(false);
      this.numberEmployees.next(response.total);
    });
  }

  public reloadEmployeeAll() {
    this.loading.next(true);
    this.employeeService.getEmployeesAll().subscribe((response: any) => {
      this.data.next(response);
      this.loading.next(false);
    });
  }
}
