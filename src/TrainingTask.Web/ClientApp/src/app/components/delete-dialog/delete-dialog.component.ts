import {Component, Inject, OnInit} from '@angular/core';
import {EmployeeService} from '../../services/employee.service';
import {MAT_DIALOG_DATA, MatDialogRef, MatSnackBar} from '@angular/material';
import {MessageService} from '../../services/message.service';


@Component({
  selector: 'app-delete-dialog',
  templateUrl: './delete-dialog.component.html',
  providers: [MessageService]
})
export class DeleteDialogComponent {

  constructor(
    private employeeService: EmployeeService,
    public dialogRef: MatDialogRef<DeleteDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: number,
    public messageService: MessageService,
  ) {
  }

  onDeleteEmployeeClick(employeeId: number) {
    this.employeeService.deleteEmployee(employeeId).subscribe(() => {
        this.closeDialog(true);
        this.messageService.openSnackBar('deleted successfully', 'Success!');
      },
      () => {
        this.closeDialog(false);
        this.messageService.openSnackBar('unable to delete', 'Error!');
      });
  }

  onCancelClick() {
    this.closeDialog(false);
  }

  closeDialog(isChangeData: boolean) {
    this.dialogRef.close(isChangeData);
  }

}
