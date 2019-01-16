import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTable, MatDialog } from '@angular/material';
import { Project } from '../../models/project';
import { ProjectDialogComponent } from '../project-dialog/project-dialog.component';
import { ProjectService } from 'src/app/services/project.service';
import { EmployeeService } from 'src/app/services/employee.service';
import { TaskService } from 'src/app/services/task.service';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})
export class ProjectsComponent implements OnInit {
  displayedColumns: string[] = ['id', 'name', 'abbreviation', 'description', 'edit', 'delete'];
  @ViewChild('table') table: MatTable<any>;

  projects: Project[] = [];
  isLoading: boolean = true;

  constructor(
    public dialog: MatDialog,
    private projectService: ProjectService,
    private employeeService: EmployeeService,
    private taskService: TaskService,
  ) { }

  ngOnInit() {
    this.projectService.getProjects()
      .subscribe(data => {
        this.isLoading = false;
        this.projects = data;
      });
  }

  openAddProjectDialog(): void {
    let project: Project = new Project;
    project.tasks = [];

    let dialogRef = this.dialog.open(ProjectDialogComponent, {
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

  addProject(project: Project): void {
    this.projectService.createProject(project)
      .subscribe(project => {
        this.projects.push(project);
        this.table.renderRows();
      });
  }

  openEditProjectDialog(project: Project): void {
    this.taskService.getTasksByProject(project.id)
      .subscribe(data => {
        project.tasks = data;
        let dialogRef = this.dialog.open(ProjectDialogComponent, {
          width: '1000px',
          data: {
            title: 'Edit Project',
            project: { ...project },
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

  editProject(project: Project): void {
    this.projectService.updateProject(project)
      .subscribe(() => {
        this.projects = this.projects.map(p => {
          return (p.id !== project.id) ? p : project;
        });
        this.table.renderRows();
      });
  }

  onDeleteProjectClick(projectId: number): void {
    this.projectService.deleteProject(projectId)
      .subscribe(() => {
        this.projects = this.projects.filter(p => p.id !== projectId);
        this.table.renderRows();
      });
  }
}