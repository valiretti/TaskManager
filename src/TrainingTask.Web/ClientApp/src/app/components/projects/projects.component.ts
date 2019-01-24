import {Component, OnInit} from '@angular/core';
import {Project} from '../../models/project';
import {ProjectService} from 'src/app/services/project.service';
import {ProjectDataSource} from '../../services/project.datasource';
import {DialogService} from '../../services/dialog.service';
import {MessageService} from '../../services/message.service';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css'],
  providers: [ProjectDataSource]
})
export class ProjectsComponent implements OnInit {
  displayedColumns: string[] = ['id', 'name', 'abbreviation', 'description', 'edit', 'delete'];

  constructor(
    public projectDataSource: ProjectDataSource,
    public dialogService: DialogService,
    private projectService: ProjectService,
    public messageService: MessageService,
  ) {
  }

  ngOnInit() {
    this.projectDataSource.reloadProject();
  }

  onOpenDetailsProjectClick(project: Project = new Project()) {
    this.dialogService
      .openDetailsProjectDialog(project)
      .afterClosed()
      .subscribe(this.handleCloseDialog);
  }

  onDeleteProjectClick(projectId: number) {
    this.dialogService
      .openDeleteDialog()
      .afterClosed()
      .subscribe((isDelete) => {
        if (isDelete) {
          this.projectService.deleteProject(projectId).subscribe(
            () => {
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
      this.projectDataSource.reloadProject();
    }
  };
}
