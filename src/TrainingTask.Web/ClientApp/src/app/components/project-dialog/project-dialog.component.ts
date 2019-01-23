import {Component, Inject} from '@angular/core';
import {MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';
import {Project} from '../../models/project';
import {EmployeeDataSource} from '../../services/employee.datasource';
import {ProjectDataSource} from '../../services/project.datasource';
import {ProjectService} from '../../services/project.service';
import {MessageService} from '../../services/message.service';

@Component({
  selector: 'app-project-dialog',
  templateUrl: './project-dialog.component.html'
  // TODO: приложение не должно содержать пустых файлов
})
// TODO: Эта компонента дублирует компоненту TasksComponent!
export class ProjectDialogComponent {
  constructor(
    private projectService: ProjectService,
    public dialogRef: MatDialogRef<ProjectDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Project,
    public messageService: MessageService,
  ) {
  }

  setTasksByProject(tasks) {
    this.data.tasks = tasks;
  }

  onSaveProjectClick(project: Project) {
    this.projectService.saveProject(project).subscribe(
      () => {
        this.closeDialog(true);
        this.messageService.openSnackBar('saved successfully', 'Success!');
      },
      () => {
        this.closeDialog(false);
        this.messageService.openSnackBar('unable to save', 'Error!');
      }
    );
  }

  onCancelClick() {
    this.closeDialog(false);
  }

  closeDialog(isChangeData: boolean) {
    this.dialogRef.close(isChangeData);
  }

}
