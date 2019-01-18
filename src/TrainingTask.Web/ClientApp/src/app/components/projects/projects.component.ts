import {Component, OnInit, ViewChild} from '@angular/core';
import {MatTable, MatDialog} from '@angular/material';
import {Project} from '../../models/project';
import {ProjectDialogComponent} from '../project-dialog/project-dialog.component';
import {ProjectService} from 'src/app/services/project.service';
import {EmployeeService} from 'src/app/services/employee.service';
import {TaskService} from 'src/app/services/task.service';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})
export class ProjectsComponent implements OnInit {
  displayedColumns: string[] = ['id', 'name', 'abbreviation', 'description', 'edit', 'delete'];
  @ViewChild('table') table: MatTable<any>;

  projects: Project[] = [];
  isLoading = true;

  constructor(
    public dialog: MatDialog,
    private projectService: ProjectService,
    private employeeService: EmployeeService,
    private taskService: TaskService,
  ) {
  }

  ngOnInit() {
    this.projectService.getProjects()
      .subscribe(data => {
        this.isLoading = false;
        this.projects = data;
      });
  }

  openAddProjectDialog() {
    const project: Project = new Project;
    project.tasks = [];

    const dialogRef = this.dialog.open(ProjectDialogComponent, {
      width: '1000px',
      data: {
        title: 'Add Project',
        project: project,
        displayedColumns: ['taskId', 'taskName', 'startDate', 'finishDate', 'employees', 'status', 'edit', 'delete'],
        projectService: this.projectService,
        employeeService: this.employeeService,
      },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.addProject(result);
      }
    });
  }

  addProject(project: Project) {
    this.projectService.createProject(project)
      .subscribe(responseProject => {
        this.projects.push(responseProject);
        this.table.renderRows();
      });
  }

  openEditProjectDialog(project: Project) {
    this.taskService.getTasksByProject(project.id)
      .subscribe(data => {
        project.tasks = data;
        const dialogRef = this.dialog.open(ProjectDialogComponent, {
          width: '1000px',
          data: {
            title: 'Edit Project',
            project: {...project},
            displayedColumns: ['taskId', 'taskName', 'startDate', 'finishDate', 'employees', 'status', 'edit', 'delete'],
            projectService: this.projectService,
            employeeService: this.employeeService,
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

  editProject(project: Project) {
    this.projectService.updateProject(project)
      .subscribe(() => {
        this.projects = this.projects.map(p => {
          return (p.id !== project.id) ? p : project;
        });
        this.table.renderRows();
      });
  }

  onDeleteProjectClick(projectId: number) {
    this.projectService.deleteProject(projectId)
      .subscribe(() => {
        this.projects = this.projects.filter(p => p.id !== projectId);
        this.table.renderRows();
      });
  }
}
