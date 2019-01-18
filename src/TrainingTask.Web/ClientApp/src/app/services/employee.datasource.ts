import {DataSource} from '@angular/cdk/table';
import {Employee} from '../models/employee';
import {CollectionViewer} from '@angular/cdk/collections';
import {BehaviorSubject, Observable, Subject} from 'rxjs';
import {Injectable} from '@angular/core';
import {EmployeeService} from './employee.service';

@Injectable()
export class EmployeeDataSource extends DataSource<Employee> {

  public loading = new BehaviorSubject<boolean>(false);

  private data: BehaviorSubject<Array<Employee>> = new BehaviorSubject([]);

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

  public reloadEmployee() {
    this.loading.next(true);
    this.employeeService.getEmployees().subscribe((response: Array<Employee>) => {
      this.data.next(response);
      this.loading.next(false);
    });
  }
}
