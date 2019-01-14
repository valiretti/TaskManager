import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HttpClientModule } from '@angular/common/http';
import { EmployeesComponent } from './employees/employees.component';
import { TasksComponent } from './tasks/tasks.component';
import { ProjectsComponent } from './projects/projects.component';
import { NotFoundComponent } from './not-found/not-found.component';

import { FormsModule } from '@angular/forms';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import { EmployeeDialogComponent } from './employee-dialog/employee-dialog.component';
import { ProjectDialogComponent } from './project-dialog/project-dialog.component';
import { TaskDialogComponent } from './task-dialog/task-dialog.component';
import { EnumKeyValuePipe } from './enum-key-value.pipe';

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
