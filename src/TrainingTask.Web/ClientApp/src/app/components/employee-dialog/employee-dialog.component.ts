import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-employee-dialog',
  templateUrl: './employee-dialog.component.html'
})
export class EmployeeDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<EmployeeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) { }

  ngOnInit() {
  }

  onCancelClick(): void {
    this.closeDialog();
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

}
