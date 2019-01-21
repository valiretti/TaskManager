import {Component, OnInit, Type, ViewChild} from '@angular/core';
import {Employee} from '../../models/employee';
import {EmployeeDataSource} from '../../services/employee.datasource';
import {DialogService} from '../../services/dialog.service';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  /*
   TODO: В реальных приложениях HttpService никогда не используется напрямую,
   В таком контексте использования сам по себе HttpService не будет являться синглтоном что может привести к неожиданным
   результаттам работы приложенияб когда будет добавлена обработка заголовков или ошибок сервера
  */
  providers: [EmployeeDataSource]
})
// TODO: Компонента перегружена логикой.
// TODO: Всю логику, которая не имеет непосредственного отношения к шаблону нужно выносить в сервисы
// TODO: Обязательно нужно декомпозировать элементы приложения
export class EmployeesComponent implements OnInit {
  displayedColumns: string[] = ['id', 'firstName', 'lastName', 'patronymic', 'position', 'edit', 'delete'];

  constructor(
    public dataSource: EmployeeDataSource,
    public dialogService: DialogService,
  ) {
  }

  ngOnInit() {
    this.dataSource.reloadEmployee();
  }

  onOpenDetailsEmployeeClick(employee: Employee = new Employee()) {
    this.dialogService
      .openDetailsEmployeeDialog(employee)
      .afterClosed()
      .subscribe(this.handleCloseDialog);
  }

  onDeleteEmployeeClick(employeeId: number) {
    this.dialogService
      .openDeleteEmployeeDialog(employeeId)
      .afterClosed()
      .subscribe(this.handleCloseDialog);
  }

  handleCloseDialog = (isDataChanged: boolean) => {
    if (isDataChanged) {
      this.dataSource.reloadEmployee();
    }
  };
}
