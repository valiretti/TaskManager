import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog, MatTable } from '@angular/material';
import { Task, Status } from '../task';
import { TaskDialogComponent } from '../task-dialog/task-dialog.component';
import { Employee } from '../employee';
import { Project } from '../project';

@Component({
  selector: 'app-project-dialog',
  templateUrl: './project-dialog.component.html',
  // TODO: приложение не должно содержать пустых файлов
  styleUrls: ['./project-dialog.component.css']
})
// TODO: Эта компонента дублирует компоненту TasksComponent!
export class ProjectDialogComponent implements OnInit {
  employeeList: Employee[] = [];
  projectList: Project[] = [];
  status = Status;
  @ViewChild('tableTasksByProject') table: MatTable<any>;

  constructor(
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<ProjectDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) { }

  ngOnInit() {
    this.data.httpService.getEmployees()
      .subscribe(data => this.employeeList = data);

    this.data.httpService.getProjects()
      .subscribe(data => this.projectList = data);
  }

  onCancelClick(): void {
    this.closeDialog();
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  openAddTaskDialog(): void {
    let task: Task = new Task;
    task.projectId = this.data.project.id;

    let dialogRef = this.dialog.open(TaskDialogComponent, {
      width: '500px',
      data: {
        title: 'Add Task',
        task: task,
        status: this.status,
        projectList: this.projectList,
        employeeList: this.employeeList,
        projectReadOnly: true
      },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        result.fullNames = this.employeeList
          .filter(f => result.employees.indexOf(f.id) >= 0)
          .map(e => `${e.firstName} ${e.lastName} ${e.patronymic}`);

        this.data.project.tasks.push(result);
        this.table.renderRows();
      }
    });
  }

  openEditTaskDialog(task: Task): void {
    let dialogRef = this.dialog.open(TaskDialogComponent, {
      width: '500px',
      data: {
        title: 'Edit Task',
        task: { ...task },
        status: this.status,
        projectList: this.projectList,
        employeeList: this.employeeList,
        projectReadOnly: true
      },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // TODO: некорректное присваивание значение полю readonly
        result.statusName = Status[result.status];

        /* TODO: хранить отдельно сконкатенированное имя - плохая практика, так как в этом случае надо будет заморачиваться над
          обновлением этого поля. Как вариант можно использовать геттер который будет возвращать полное имя либо написатьпайп в котором
          будет добавлена логика конкатенации.
         */
        result.fullNames = this.employeeList
          .filter(f => result.employees.indexOf(f.id) >= 0)
          .map(e => `${e.firstName} ${e.lastName} ${e.patronymic}`);

        /* TODO: Список должен обновляться серверными даннымиб и тут 2 варианта:
         1. Договориться с бэком, что он будет присылать сохранённую модель в PUT и POST методах
         2. Перезапрашивать список после сохранения или добавления элемента
        */
        this.data.project.tasks = this.data.project.tasks.map((t: Task) => {
          return (t.id !== result.id) ? t : result;
        });
      }
    });
  }

  onDeleteTaskClick(taskId: number): void {
    this.data.project.tasks = this.data.project.tasks.filter((t: Task) => t.id !== taskId);
  }

}
