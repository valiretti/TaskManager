import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpService } from '../http.service';
import { MatTable, MatDialog } from '@angular/material';
import { Project } from '../project';
import { ProjectDialogComponent } from '../project-dialog/project-dialog.component';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css'],
  providers: [HttpService]
})
export class ProjectsComponent implements OnInit {
  displayedColumns: string[] = ['id', 'name', 'abbreviation', 'description', 'edit', 'delete'];
  @ViewChild('table') table: MatTable<any>;

  projects: Project[] = [];
  project: Project = new Project;

  constructor(
    public dialog: MatDialog,
    private httpService: HttpService
  ) { }

  ngOnInit() {
    this.httpService.getProjects()
      .subscribe(data => this.projects = data);
  }

  openAddProjectDialog(): void {
    let dialogRef = this.dialog.open(ProjectDialogComponent, {
      width: '500px',
      data: {
        title: 'Add Project',
        project: this.project,
      },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.addProject(result);
      }
    });
  }

  addProject(project: Project): void {
    this.httpService.addProject(project)
      .subscribe(project => {
        this.projects.push(project);
        this.table.renderRows();
      });
  }

  openEditProjectDialog(project: Project): void {
    this.httpService.getTasksByProject(project.id)
      .subscribe(data => {
        project.tasks = data;
        let dialogRef = this.dialog.open(ProjectDialogComponent, {
          width: '1000px',
          data: {
            displayTaskTable: true,
            title: 'Edit Project',
            project: { ...project },
            displayedColumns: ['taskId', 'taskName', 'startDate', 'finishDate', 'employees', 'status', 'edit', 'delete'],
          },
          disableClose: true
        });

        dialogRef.afterClosed().subscribe(result => {
          if (result) {
            this.editProject(result);
          }
        });
      });
  }

  editProject(project: Project): void {
    this.httpService.updateProject(project)
      .subscribe(() => {
        this.projects = this.projects.map(p => {
          return (p.id !== project.id) ? p : project;
        });
        this.table.renderRows();
      });
  }

  onDeleteProjectClick(projectId: number): void {
    this.httpService.deleteProject(projectId)
      .subscribe(() => {
        this.projects = this.projects.filter(p => p.id !== projectId);
        this.table.renderRows();
      });
  }
}