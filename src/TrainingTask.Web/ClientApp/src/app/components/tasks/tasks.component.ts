import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpService } from '../../services/http.service';
import { Task } from '../../models/task';
import { MatTable, MatDialog } from '@angular/material';
import { TaskDialogComponent } from '../task-dialog/task-dialog.component';
import { Employee } from '../../models/employee';
import { Project } from '../../models/project';
import { StatusTask } from '../../models/statusTaskEnum';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  providers: [HttpService]
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
    private httpService: HttpService
  ) { }

  ngOnInit() {
    this.httpService.getTasks()
      .subscribe(data => {
        this.tasks = data;
        this.isLoading = false;
      });

    this.httpService.getEmployees()
      .subscribe(data => this.employeeList = data);

    this.httpService.getProjects()
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
    this.httpService.addTask(task)
      .subscribe(task => {
        this.tasks.push(task);
        this.table.renderRows();
      });
  }

  openEditTaskDialog(task: Task): void {
    this.httpService.getTasksById(task.id)
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
    this.httpService.updateTask(task)
      .subscribe(() => {
        this.tasks = this.tasks.map(t => {
          return (t.id !== task.id) ? t : task;
        });
        this.table.renderRows();
      });
  }

  onDeleteTaskClick(taskId: number): void {
    this.httpService.deleteTask(taskId)
      .subscribe(() => {
        this.tasks = this.tasks.filter(t => t.id !== taskId);
        this.table.renderRows();
      });
  }

}