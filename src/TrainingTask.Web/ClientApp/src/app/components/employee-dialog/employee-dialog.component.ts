import {Component, Inject} from '@angular/core';
import {MatDialogRef, MAT_DIALOG_DATA, MatTable} from '@angular/material';
import {Employee} from '../../models/employee';
import {EmployeeService} from '../../services/employee.service';
import {MessageService} from '../../services/message.service';

@Component({
  selector: 'app-employee-dialog',
  templateUrl: './employee-dialog.component.html',
  providers: [MessageService]
})
export class EmployeeDialogComponent {

  constructor(
    private employeeService: EmployeeService,
    public dialogRef: MatDialogRef<EmployeeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Employee,
    public messageService: MessageService,
  ) {
  }

  onSaveEmployeeClick(employee: Employee) {
    this.employeeService.saveEmployee(employee).subscribe(() => {
        this.closeDialog(true);
        this.messageService.openSnackBar('saved successfully', 'Success!');
      },
      () => {
        this.closeDialog(false);
        this.messageService.openSnackBar('unable to save', 'Error!');
      });
  }

  onCancelClick() {
    this.closeDialog(false);
  }

  closeDialog(isChangeData: boolean) {
    this.dialogRef.close(isChangeData);
  }

}
