import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './modules/app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { EmployeesComponent } from './components/employees/employees.component';
import { TasksComponent } from './components/tasks/tasks.component';
import { ProjectsComponent } from './components/projects/projects.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './modules/material.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import { EmployeeDialogComponent } from './components/employee-dialog/employee-dialog.component';
import { ProjectDialogComponent } from './components/project-dialog/project-dialog.component';
import { TaskDialogComponent } from './components/task-dialog/task-dialog.component';
import { EnumKeyValuePipe } from './pipes/enum-key-value.pipe';


@NgModule({
  declarations: [
    AppComponent,
    EmployeesComponent,
    TasksComponent,
    ProjectsComponent,
    NotFoundComponent,
    EmployeeDialogComponent,
    ProjectDialogComponent,
    TaskDialogComponent,
    EnumKeyValuePipe,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MaterialModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    FlexLayoutModule
  ],
  entryComponents: [
    EmployeeDialogComponent,
    ProjectDialogComponent,
    TaskDialogComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
