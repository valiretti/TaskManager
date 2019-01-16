import { Component, OnInit, ViewChild } from '@angular/core';
import { Task } from '../../models/task';
import { MatTable, MatDialog } from '@angular/material';
import { TaskDialogComponent } from '../task-dialog/task-dialog.component';
import { Employee } from '../../models/employee';
import { Project } from '../../models/project';
import { StatusTask } from '../../models/statusTaskEnum';
import { EmployeeService } from 'src/app/services/employee.service';
import { ProjectService } from 'src/app/services/project.service';
import { TaskService } from 'src/app/services/task.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html'
})
export class TasksComponent implements OnInit {
  displayedColumns: string[] = ['id', 'projectName', 'name', 'startDate', 'finishDate', 'employees', 'status', 'edit', 'delete'];
  @ViewChild('table') table: MatTable<any>;

  tasks: Task[] = [];
  task: Task = new Task;
  employeeList: Employee[] = [];
  projectList: Project[] = [];
  status = StatusTask;
  isLoading: boolean = true;

  constructor(
    public dialog: MatDialog,
    private employeeService: EmployeeService,
    private projectService: ProjectService,
    private taskService: TaskService,
  ) { }

  ngOnInit() {
    this.taskService.getTasks()
      .subscribe((data: Task[]) => {
        this.tasks = data;
        this.isLoading = false;
      });

    this.employeeService.getEmployees()
      .subscribe(data => this.employeeList = data);

    this.projectService.getProjects()
      .subscribe(data => this.projectList = data);
  }

  openAddTaskDialog(): void {
    let dialogRef = this.dialog.open(TaskDialogComponent, {
      width: '500px',
      data: {
        title: 'Add Task',
        task: this.task,
        status: this.status,
        projectList: this.projectList,
        employeeList: this.employeeList
      },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.addTask(result);
      }
    });
  }

  addTask(task: Task): void {
    this.taskService.createTask(task)
      .subscribe(task => {
        this.tasks.push(task);
        this.table.renderRows();
      });
  }

  openEditTaskDialog(task: Task): void {
    this.taskService.getTaskById(task.id)
      .subscribe(data => {
        let dialogRef = this.dialog.open(TaskDialogComponent, {
          width: '500px',
          data: {
            title: 'Edit Task',
            task: data,
            status: this.status,
            projectList: this.projectList,
            employeeList: this.employeeList
          },
          disableClose: true
        });

        dialogRef.afterClosed().subscribe(result => {
          if (result) {
            result.fullNames = this.employeeList
              .filter(f => result.employees.indexOf(f.id) >= 0)
              .map(e => `${e.firstName} ${e.lastName} ${e.patronymic}`);

            let currentProject = this.projectList
              .find(p => p.id === result.projectId);

            if (currentProject.abbreviation) {
              result.projectAbbreviation = currentProject.abbreviation;
            }

            this.editTask(result);
          }
        });
      });
  }

  editTask(task: Task): void {
    this.taskService.updateTask(task)
      .subscribe(() => {
        this.tasks = this.tasks.map(t => {
          return (t.id !== task.id) ? t : task;
        });
        this.table.renderRows();
      });
  }

  onDeleteTaskClick(taskId: number): void {
    this.taskService.deleteTask(taskId)
      .subscribe(() => {
        this.tasks = this.tasks.filter(t => t.id !== taskId);
        this.table.renderRows();
      });
  }

}