import {Component, OnInit, Type, ViewChild} from '@angular/core';
import {Employee} from '../../models/employee';
import {EmployeeDataSource} from '../../services/employee.datasource';
import {DialogService} from '../../services/dialog.service';
import {TaskService} from '../../services/task.service';
import {MessageService} from '../../services/message.service';
import {EmployeeService} from '../../services/employee.service';

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
    private employeeService: EmployeeService,
    public messageService: MessageService,
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
      .openDeleteDialog()
      .afterClosed()
      .subscribe((isDelete) => {
        if (isDelete) {
          this.employeeService.deleteEmployee(employeeId).subscribe(() => {
              this.handleCloseDialog(true);
              this.messageService.openSnackBar('deleted successfully', 'Success!');
            },
            () => {
              this.messageService.openSnackBar('unable to delete', 'Error!');
            });
        }
      });
  }

  handleCloseDialog = (isDataChanged: boolean) => {
    if (isDataChanged) {
      this.dataSource.reloadEmployee();
    }
  };
}
